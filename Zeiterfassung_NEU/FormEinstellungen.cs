using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Zeiterfassung_NEU
{
    /// <summary>
    /// Einstellungsfenster, im Code aufgebaut statt mit dem Designer.
    /// </summary>
    public class FormEinstellungen : Form
    {
        private static readonly TimeSpan ZeitAngestellte = new TimeSpan(8, 40, 0);
        private static readonly TimeSpan ZeitUnter18 = new TimeSpan(9, 0, 0);

        private RadioButton rbAngestellte;
        private RadioButton rbUnter18;
        private RadioButton rbBenutzerdefiniert;
        private TextBox tbBenutzerdefiniert;

        private TextBox tbMaxArbeitszeit;
        private TextBox tbWarnzeit;
        private CheckBox cbStartMinimiert;
        private NumericUpDown nudAnzahlTage;

        private TextBox tbIconPfad;
        private Button btnIconDurchsuchen;

        private TextBox tbDbPfad;
        private Button btnDbDurchsuchen;

        private Button btnSpeichern;
        private Button btnAbbrechen;

        // damit Form1 Icon und Datenbank ohne Neustart neu laden kann
        public event EventHandler IconOderDatenbankGeaendert;

        public FormEinstellungen()
        {
            Text = "Einstellungen";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(460, 585);

            // ---------- Tägliche Arbeitszeit ----------
            var lbUeberschrift = new Label
            {
                Text = "Tägliche Arbeitszeit:",
                Location = new Point(20, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            rbAngestellte = new RadioButton
            {
                Text = "Standard-Arbeitszeit (8:40 Std)",
                Location = new Point(20, 50),
                AutoSize = true
            };

            rbUnter18 = new RadioButton
            {
                Text = "Unter 18 Jahren (9:00 Std)",
                Location = new Point(20, 80),
                AutoSize = true
            };

            rbBenutzerdefiniert = new RadioButton
            {
                Text = "Benutzerdefiniert:",
                Location = new Point(20, 112),
                AutoSize = true
            };
            rbBenutzerdefiniert.CheckedChanged += (s, e) =>
            {
                tbBenutzerdefiniert.Enabled = rbBenutzerdefiniert.Checked;
            };

            tbBenutzerdefiniert = new TextBox
            {
                Location = new Point(180, 110),
                Width = 80,
                Enabled = false
            };

            var lbHinweis = new Label
            {
                Text = "Format: HH:mm, z.B. 08:40",
                Location = new Point(20, 145),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F)
            };

            // ---------- Maximale Arbeitszeit & Warnschwelle ----------
            var lbMaxArbeitszeit = new Label
            {
                Text = "Maximale Arbeitszeit (HH:mm):",
                Location = new Point(20, 180),
                AutoSize = true
            };

            tbMaxArbeitszeit = new TextBox
            {
                Location = new Point(250, 177),
                Width = 80
            };

            var lbWarnzeit = new Label
            {
                Text = "Warnung ab (HH:mm):",
                Location = new Point(20, 215),
                AutoSize = true
            };

            tbWarnzeit = new TextBox
            {
                Location = new Point(250, 212),
                Width = 80
            };

            var lbWarnHinweis = new Label
            {
                Text = "Ab dieser gearbeiteten Zeit kommt eine Warnung.",
                Location = new Point(20, 240),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F)
            };

            // ---------- Start-Verhalten ----------
            cbStartMinimiert = new CheckBox
            {
                Text = "Beim Start direkt minimiert im Tray starten",
                Location = new Point(20, 275),
                AutoSize = true
            };

            // ---------- Einträge pro Seite ----------
            var lbAnzahlTage = new Label
            {
                Text = "Anzahl angezeigter Einträge pro Seite:",
                Location = new Point(20, 315),
                AutoSize = true
            };

            nudAnzahlTage = new NumericUpDown
            {
                Location = new Point(270, 312),
                Width = 70,
                Minimum = 1,
                Maximum = 3650,
                Value = 50
            };

            // ---------- Icon-Pfad ----------
            var lbIconUeberschrift = new Label
            {
                Text = "Tray-Icon:",
                Location = new Point(20, 355),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            tbIconPfad = new TextBox
            {
                Location = new Point(20, 380),
                Width = 340
            };

            btnIconDurchsuchen = new Button
            {
                Text = "...",
                Location = new Point(368, 378),
                Size = new Size(35, 25)
            };
            btnIconDurchsuchen.Click += BtnIconDurchsuchen_Click;

            var lbIconHinweis = new Label
            {
                Text = "Leer lassen für das Standard-Icon.",
                Location = new Point(20, 408),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F)
            };

            // ---------- Datenbank-Pfad ----------
            var lbDbUeberschrift = new Label
            {
                Text = "Datenbank-Datei:",
                Location = new Point(20, 440),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            tbDbPfad = new TextBox
            {
                Location = new Point(20, 465),
                Width = 340
            };

            btnDbDurchsuchen = new Button
            {
                Text = "...",
                Location = new Point(368, 463),
                Size = new Size(35, 25)
            };
            btnDbDurchsuchen.Click += BtnDbDurchsuchen_Click;

            var lbDbHinweis = new Label
            {
                Text = "Leer lassen für den Standardpfad.",
                Location = new Point(20, 493),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F)
            };

            var lbDbHinweis2 = new Label
            {
                Text = "Vorhandene Einträge werden beim Ändern nicht automatisch übernommen.",
                Location = new Point(20, 508),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F)
            };

            // ---------- Buttons ----------
            btnSpeichern = new Button
            {
                Text = "Speichern",
                Location = new Point(180, 535),
                Size = new Size(90, 35)
            };
            btnSpeichern.Click += BtnSpeichern_Click;

            btnAbbrechen = new Button
            {
                Text = "Abbrechen",
                Location = new Point(280, 535),
                Size = new Size(90, 35)
            };
            btnAbbrechen.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                lbUeberschrift, rbAngestellte, rbUnter18, rbBenutzerdefiniert,
                tbBenutzerdefiniert, lbHinweis,
                lbMaxArbeitszeit, tbMaxArbeitszeit,
                lbWarnzeit, tbWarnzeit, lbWarnHinweis,
                cbStartMinimiert,
                lbAnzahlTage, nudAnzahlTage,
                lbIconUeberschrift, tbIconPfad, btnIconDurchsuchen, lbIconHinweis,
                lbDbUeberschrift, tbDbPfad, btnDbDurchsuchen, lbDbHinweis, lbDbHinweis2,
                btnSpeichern, btnAbbrechen
            });

            AktuelleWerteLaden();
        }

        private void AktuelleWerteLaden()
        {
            TimeSpan arbeitszeit = EinstellungenVerwaltung.ArbeitszeitProTag;

            if (arbeitszeit == ZeitAngestellte)
            {
                rbAngestellte.Checked = true;
                tbBenutzerdefiniert.Text = ZeitAngestellte.ToString(@"hh\:mm");
            }
            else if (arbeitszeit == ZeitUnter18)
            {
                rbUnter18.Checked = true;
                tbBenutzerdefiniert.Text = ZeitUnter18.ToString(@"hh\:mm");
            }
            else
            {
                rbBenutzerdefiniert.Checked = true;
                tbBenutzerdefiniert.Text = arbeitszeit.ToString(@"hh\:mm");
            }

            tbMaxArbeitszeit.Text = EinstellungenVerwaltung.MaxArbeitszeit.ToString(@"hh\:mm");
            tbWarnzeit.Text = EinstellungenVerwaltung.WarnzeitVorMax.ToString(@"hh\:mm");
            cbStartMinimiert.Checked = EinstellungenVerwaltung.StartMinimiert;

            int aktuelleAnzahl = EinstellungenVerwaltung.AnzahlTageAnzeige;
            nudAnzahlTage.Value = Math.Min(nudAnzahlTage.Maximum, Math.Max(nudAnzahlTage.Minimum, aktuelleAnzahl));

            tbIconPfad.Text = EinstellungenVerwaltung.IconPfad;
            tbDbPfad.Text = EinstellungenVerwaltung.DatenbankPfad;
        }

        private void BtnIconDurchsuchen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title = "Icon auswählen",
                Filter = "Icon-Dateien (*.ico)|*.ico|Alle Dateien (*.*)|*.*",
                CheckFileExists = true
            })
            {
                if (!string.IsNullOrWhiteSpace(tbIconPfad.Text) && File.Exists(tbIconPfad.Text))
                {
                    dlg.InitialDirectory = Path.GetDirectoryName(tbIconPfad.Text);
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    tbIconPfad.Text = dlg.FileName;
                }
            }
        }

        private void BtnDbDurchsuchen_Click(object sender, EventArgs e)
        {
            // Datei muss nicht existieren, so kann man auch eine neue Datenbank anlegen
            using (var dlg = new OpenFileDialog
            {
                Title = "Datenbank-Datei auswählen",
                Filter = "SQLite-Datenbank (*.db)|*.db|Alle Dateien (*.*)|*.*",
                CheckFileExists = false,
                FileName = "zeiten.db"
            })
            {
                if (!string.IsNullOrWhiteSpace(tbDbPfad.Text))
                {
                    string ordner = Path.GetDirectoryName(tbDbPfad.Text);
                    if (!string.IsNullOrEmpty(ordner) && Directory.Exists(ordner))
                    {
                        dlg.InitialDirectory = ordner;
                    }
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    tbDbPfad.Text = dlg.FileName;
                }
            }
        }

        private void BtnSpeichern_Click(object sender, EventArgs e)
        {
            // ---------- Arbeitszeit ----------
            TimeSpan neueArbeitszeit;

            if (rbAngestellte.Checked)
            {
                neueArbeitszeit = ZeitAngestellte;
            }
            else if (rbUnter18.Checked)
            {
                neueArbeitszeit = ZeitUnter18;
            }
            else if (!TimeSpan.TryParse(tbBenutzerdefiniert.Text, out neueArbeitszeit))
            {
                ZeigeUngueltigeEingabe("Arbeitszeit");
                return;
            }

            // ---------- Maximale Arbeitszeit ----------
            if (!TimeSpan.TryParse(tbMaxArbeitszeit.Text, out TimeSpan neueMaxArbeitszeit))
            {
                ZeigeUngueltigeEingabe("Maximale Arbeitszeit");
                return;
            }

            // ---------- Warnschwelle ----------
            if (!TimeSpan.TryParse(tbWarnzeit.Text, out TimeSpan neueWarnzeit))
            {
                ZeigeUngueltigeEingabe("Warnung ab");
                return;
            }

            // Warnung muss vor der Maximalzeit kommen
            if (neueWarnzeit > neueMaxArbeitszeit)
            {
                MessageBox.Show(
                    "Die Warnschwelle sollte nicht größer als die maximale Arbeitszeit sein.",
                    "Ungültige Eingabe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // ---------- Icon-Pfad ----------
            string neuerIconPfad = tbIconPfad.Text.Trim();
            if (!string.IsNullOrEmpty(neuerIconPfad) &&
                !neuerIconPfad.EndsWith(".ico", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Der Icon-Pfad muss auf eine .ico-Datei zeigen (oder leer sein für den Standard).",
                    "Ungültiger Pfad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // ---------- Datenbank-Pfad ----------
            string neuerDbPfad = tbDbPfad.Text.Trim();
            if (!string.IsNullOrEmpty(neuerDbPfad) &&
                !neuerDbPfad.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Der Datenbank-Pfad muss auf eine .db-Datei zeigen (oder leer sein für den Standard).",
                    "Ungültiger Pfad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool iconOderDbGeaendert =
                neuerIconPfad != EinstellungenVerwaltung.IconPfad ||
                neuerDbPfad != EinstellungenVerwaltung.DatenbankPfad;

            try
            {
                EinstellungenVerwaltung.ArbeitszeitProTag = neueArbeitszeit;
                EinstellungenVerwaltung.MaxArbeitszeit = neueMaxArbeitszeit;
                EinstellungenVerwaltung.WarnzeitVorMax = neueWarnzeit;
                EinstellungenVerwaltung.StartMinimiert = cbStartMinimiert.Checked;
                EinstellungenVerwaltung.AnzahlTageAnzeige = (int)nudAnzahlTage.Value;
                EinstellungenVerwaltung.IconPfad = neuerIconPfad;
                EinstellungenVerwaltung.DatenbankPfad = neuerDbPfad;

                EinstellungenVerwaltung.Save();

                MessageBox.Show(
                    "Einstellungen gespeichert.",
                    "Gespeichert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (iconOderDbGeaendert)
                {
                    IconOderDatenbankGeaendert?.Invoke(this, EventArgs.Empty);
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Fehler beim Speichern:\n" + ex.Message,
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ZeigeUngueltigeEingabe(string feldname)
        {
            MessageBox.Show(
                $"Ungültige Zeit bei \"{feldname}\". Bitte im Format HH:mm eingeben, z.B. 08:40.",
                "Ungültige Eingabe",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
