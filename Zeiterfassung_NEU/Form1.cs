using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Zeiterfassung_NEU
{
    public partial class Form1 : Form
    {
        // true nur über Menü "Beenden", sonst gehen X und Minimieren ins Tray
        private bool wirdBeendet = false;

        // damit die Warnung nur einmal am Tag kommt
        private DateTime? warnungGezeigtAm = null;
        private System.Windows.Forms.Timer warnTimer;

        // 0 = neueste Einträge
        private int aktuelleSeite = 0;

        public Form1()
        {
            InitializeComponent();

            // muss zuerst passieren, alles andere braucht die Einstellungen
            EinstellungenVerwaltung.Load();

            if (EinstellungenVerwaltung.StartMinimiert)
            {
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
            }

            // fehlt die Icon-Datei, einfach das Windows-Standardicon nehmen
            string iconPath = EinstellungenVerwaltung.IconPfadEffektiv;
            trayIcon.Icon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Application;

            // jede Minute prüfen, ob gewarnt werden muss
            warnTimer = new System.Windows.Forms.Timer { Interval = 60000 };
            warnTimer.Tick += WarnTimer_Tick;
            warnTimer.Start();
        }

        private void WarnTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var heutiger = Funktionen.heutigenEintragLaden();
                if (heutiger == null) return;

                var (_, ankunftStr, _) = heutiger.Value;
                if (!TimeSpan.TryParse(ankunftStr, out TimeSpan ankunftZeit)) return;

                TimeSpan vergangen = DateTime.Now.TimeOfDay - ankunftZeit;
                if (vergangen < TimeSpan.Zero) return; // nach Mitternacht

                bool heuteSchonGewarnt = warnungGezeigtAm == DateTime.Today;

                if (vergangen >= EinstellungenVerwaltung.WarnzeitVorMax && !heuteSchonGewarnt)
                {
                    warnungGezeigtAm = DateTime.Today;

                    trayIcon.ShowBalloonTip(
                        10000,
                        "Zeiterfassung",
                        $"Du arbeitest heute schon {vergangen:hh\\:mm} Std. " +
                        $"Die maximale Arbeitszeit von {EinstellungenVerwaltung.MaxArbeitszeit:hh\\:mm} Std. rückt näher.",
                        ToolTipIcon.Warning);
                }
            }
            catch
            {
                // Fehler im Timer ignorieren, nächste Minute wird neu geprüft
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string dbPath = EinstellungenVerwaltung.DatenbankPfadEffektiv;

                Funktionen.Init(dbPath);

                // auch beim minimierten Start, die MessageBox kommt trotzdem
                ZeitPruefenUndSetzen(zeigeHinweisWennBereitsVorhanden: false);

                LadeTabelle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Beim Start ist ein Fehler aufgetreten:\n" + ex.Message,
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Legt den Eintrag für heute an, falls es noch keinen gibt.
        /// Über "Zeit setzen" im Menü wird der vorhandene Eintrag nur angezeigt.
        /// </summary>
        private void ZeitPruefenUndSetzen(bool zeigeHinweisWennBereitsVorhanden)
        {
            string datum, ankunft, ende;

            if (!Funktionen.timeTaken())
            {
                (datum, ankunft, ende) = Funktionen.createData();
                MessageBox.Show("Ankunft Zeit: " + ankunft + "\n" +
                                "Arbeiten bis: " + ende);
            }
            else
            {
                var heutiger = Funktionen.heutigenEintragLaden();
                (datum, ankunft, ende) = heutiger ?? (DateTime.Now.ToString("dd.MM.yyyy"), "-", "-");

                if (zeigeHinweisWennBereitsVorhanden)
                {
                    MessageBox.Show("Für heute bereits gesetzt:\n" +
                                    "Ankunft Zeit: " + ankunft + "\n" +
                                    "Arbeiten bis: " + ende);
                }
            }

            AktuelleZeitenAmTrayIconAnzeigen(ankunft, ende);
        }

        private void AktuelleZeitenAmTrayIconAnzeigen(string ankunft, string ende)
        {
            // Tooltip vom Tray-Icon, max. 127 Zeichen
            trayIcon.Text = $"Ankunft: {ankunft}   Ende: {ende}";
        }

        private void LadeTabelle()
        {
            try
            {
                int anzahlProSeite = EinstellungenVerwaltung.AnzahlTageAnzeige;
                DataTable table = Funktionen.zeitenVonDBLadenSeite(aktuelleSeite, anzahlProSeite);
                mainTable.DataSource = table;

                AktualisiereSeitenInfo(anzahlProSeite);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Zeiten konnten nicht geladen werden:\n" + ex.Message,
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AktualisiereSeitenInfo(int anzahlProSeite)
        {
            try
            {
                int gesamt = Funktionen.anzahlEintraegeGesamt();

                int von = gesamt == 0 ? 0 : aktuelleSeite * anzahlProSeite + 1;
                int bis = Math.Min(gesamt, (aktuelleSeite + 1) * anzahlProSeite);

                lbSeitenInfo.Text = $"Zeige {von}-{bis} von {gesamt}";

                btnSeiteZurueck.Enabled = aktuelleSeite > 0;
                btnSeiteWeiter.Enabled = bis < gesamt;
            }
            catch (Exception ex)
            {
                lbSeitenInfo.Text = "Seiteninfo nicht verfügbar";
                MessageBox.Show("Seiteninfo konnte nicht ermittelt werden:\n" + ex.Message,
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeiteZurueck_Click(object sender, EventArgs e)
        {
            if (aktuelleSeite > 0)
            {
                aktuelleSeite--;
                LadeTabelle();
            }
        }

        private void btnSeiteWeiter_Click(object sender, EventArgs e)
        {
            aktuelleSeite++;
            LadeTabelle();
        }

        // ---------- Tray ----------

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Minimieren -> ins Tray statt in die Taskleiste
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                ShowInTaskbar = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // X schließt nicht, sondern minimiert ins Tray
            if (!wirdBeendet && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                ShowInTaskbar = false;
            }
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            FensterOeffnen();
        }

        private void menuEinstellungen_Click(object sender, EventArgs e)
        {
            using (var dlg = new FormEinstellungen())
            {
                dlg.IconOderDatenbankGeaendert += (s, ev) =>
                {
                    // Icon neu laden
                    string iconPath = EinstellungenVerwaltung.IconPfadEffektiv;
                    trayIcon.Icon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Application;

                    // neue Datenbank ohne Neustart verwenden
                    try
                    {
                        Funktionen.Init(EinstellungenVerwaltung.DatenbankPfadEffektiv);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Die neue Datenbank konnte nicht geöffnet werden:\n" + ex.Message,
                            "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dlg.ShowDialog();
            }

            // Anzahl pro Seite kann sich geändert haben -> wieder auf Seite 1
            aktuelleSeite = 0;
            LadeTabelle();
        }

        private void menuOeffnen_Click(object sender, EventArgs e)
        {
            FensterOeffnen();
        }

        private void FensterOeffnen()
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Activate();
        }

        private void menuZeitSetzen_Click(object sender, EventArgs e)
        {
            try
            {
                ZeitPruefenUndSetzen(zeigeHinweisWennBereitsVorhanden: true);
                LadeTabelle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler:\n" + ex.Message,
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuGehen_Click(object sender, EventArgs e)
        {
            try
            {
                var heutiger = Funktionen.heutigenEintragMitIdLaden();

                if (heutiger == null)
                {
                    MessageBox.Show(
                        "Für heute wurde noch keine Ankunftszeit gesetzt.\n" +
                        "Bitte zuerst \"Zeit setzen\" verwenden.",
                        "Kein Eintrag vorhanden",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var (id, datum, ankunft, _) = heutiger.Value;
                string jetzt = DateTime.Now.ToString("HH:mm");

                Funktionen.saveData(id, ankunft, jetzt);

                AktuelleZeitenAmTrayIconAnzeigen(ankunft, jetzt);
                LadeTabelle();

                MessageBox.Show("Gehen-Zeit gespeichert: " + jetzt,
                    "Zeit gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler:\n" + ex.Message,
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuBeenden_Click(object sender, EventArgs e)
        {
            wirdBeendet = true;
            trayIcon.Visible = false; // sonst bleibt das Icon im Tray stehen, bis man drüberfährt
            Application.Exit();
        }

        // ---------- Tabelle ----------

        private List<DataGridViewRow> GetSelectedRows()
        {
            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in mainTable.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["Check"].Value is bool b && b;
                if (isChecked)
                {
                    selectedRows.Add(row);
                }
            }
            return selectedRows;
        }

        private void bntDelete_Click(object sender, EventArgs e)
        {
            var rows = GetSelectedRows();

            if (rows.Count == 0)
            {
                MessageBox.Show("Keine Zeilen ausgewählt.");
                return;
            }

            var confirm = MessageBox.Show(
                $"{rows.Count} Zeile(n) wirklich löschen?",
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                foreach (var row in rows)
                {
                    Funktionen.deleteData(row.Cells["id"].Value.ToString());
                }

                LadeTabelle();
                lbStatus.Text = $"{rows.Count} Zeile(n) gelöscht";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Löschen fehlgeschlagen:\n" + ex.Message,
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mainTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (!(mainTable.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)) return;

            mainTable.EndEdit();

            var geklickteZelle = mainTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
            bool neuerWert = !(geklickteZelle.Value is bool b && b);

            foreach (DataGridViewCell zelle in mainTable.SelectedCells)
            {
                if (mainTable.Columns[zelle.ColumnIndex] is DataGridViewCheckBoxColumn)
                {
                    zelle.Value = neuerWert;
                }
            }

            mainTable.EndEdit();
            mainTable.RefreshEdit();
        }

        private void mainTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (mainTable.Columns[e.ColumnIndex].Name == "Datum")
            {
                var row = mainTable.Rows[e.RowIndex];

                string rowID = row.Cells["id"].Value?.ToString() ?? "";
                tbDatum.Text = "Datum: " + (row.Cells["Datum"].Value?.ToString() ?? "");
                tbAnfang.Text = (row.Cells["Anfang"].Value?.ToString() ?? "");
                tbEnde.Text =  (row.Cells["Ende"].Value?.ToString() ?? "");
                lbRowNr.Text = "ID: " + rowID;

                btnSave.Visible = true;
                tbDatum.Visible = true;
                tbAnfang.Visible = true;
                tbEnde.Visible = true;
                lbAnkunft.Visible = true;
                lbEnde.Visible = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string idText = lbRowNr.Text.Replace("ID:", "").Trim();

            if (!int.TryParse(idText, out int rowID))
            {
                lbStatus.Text = "Keine gültige ID ausgewählt.";
                return;
            }

            string neuAnfang = tbAnfang.Text;
            string neuEnde = tbEnde.Text;

            if (!TimeSpan.TryParse(neuAnfang, out TimeSpan anfangZeit))
            {
                lbStatus.Text = "Ungültige Startzeit (erwartet z.B. 08:00).";
                return;
            }

            bool endeManuellGesetzt = TimeSpan.TryParse(neuEnde, out _);

            foreach (DataGridViewRow row in mainTable.Rows)
            {
                if (row.Cells["id"].Value != null &&
                    Convert.ToInt32(row.Cells["id"].Value) == rowID)
                {
                    bool anfangGeaendert = neuAnfang != row.Cells["Anfang"].Value?.ToString();
                    bool endeGeaendert = neuEnde != row.Cells["Ende"].Value?.ToString();

                    if (anfangGeaendert && !endeGeaendert)
                    {
                        neuEnde = anfangZeit.Add(EinstellungenVerwaltung.ArbeitszeitProTag).ToString(@"hh\:mm");
                    }
                    else if (endeGeaendert && !endeManuellGesetzt)
                    {
                        lbStatus.Text = "Ungültige Endzeit (erwartet z.B. 16:40).";
                        return;
                    }

                    if (anfangGeaendert || endeGeaendert)
                    {
                        try
                        {
                            Funktionen.saveData(idText, neuAnfang, neuEnde);
                            row.Cells["Anfang"].Value = neuAnfang;
                            row.Cells["Ende"].Value = neuEnde;
                            lbStatus.Text = "Gespeichert.";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Speichern fehlgeschlagen:\n" + ex.Message,
                                "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    break;
                }
            }
        }
    }
}
