using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Zeiterfassung_NEU
{
    /// <summary>
    /// Inhalt der settings.json. Zeiten als Text ("HH:mm"), damit man die Datei
    /// auch von Hand bearbeiten kann.
    /// </summary>
    public class Einstellungen
    {
        public string ArbeitszeitProTag { get; set; } = "08:40";
        public string MaxArbeitszeit { get; set; } = "10:00";
        public string WarnzeitVorMax { get; set; } = "09:30";
        public bool StartMinimiert { get; set; } = true;
        public int AnzahlTageAnzeige { get; set; } = 50;

        // leer = Resources\icon_clock.ico neben der .exe
        public string IconPfad { get; set; } = "";

        // leer = zeiten.db in %LOCALAPPDATA%\Zeiterfassung_NEU
        public string DatenbankPfad { get; set; } = "";
    }

    /// <summary>
    /// Laden und Speichern der settings.json in %LOCALAPPDATA%\Zeiterfassung_NEU.
    /// </summary>
    internal static class EinstellungenVerwaltung
    {
        private static readonly string appOrdner = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Zeiterfassung_NEU");

        private static readonly string settingsPfad = Path.Combine(appOrdner, "settings.json");

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static Einstellungen Aktuell { get; private set; } = new Einstellungen();

        /// <summary>
        /// Fehlt die Datei oder ist sie kaputt, werden die Standardwerte genommen
        /// und neu gespeichert.
        /// </summary>
        public static void Load()
        {
            AlteDatenbankUebernehmen();

            try
            {
                if (File.Exists(settingsPfad))
                {
                    string json = File.ReadAllText(settingsPfad);
                    var geladen = JsonSerializer.Deserialize<Einstellungen>(json, jsonOptions);
                    if (geladen != null)
                    {
                        Aktuell = geladen;
                        return;
                    }
                }
            }
            catch
            {
                // weiter unten mit Standardwerten neu anlegen
            }

            Aktuell = new Einstellungen();
            Save();
        }

        // Früher lag die zeiten.db direkt in %LOCALAPPDATA%. Falls es die alte Datei
        // noch gibt, wird sie einmalig in den App-Ordner kopiert (die alte bleibt liegen).
        private static void AlteDatenbankUebernehmen()
        {
            try
            {
                string alt = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "zeiten.db");

                if (File.Exists(alt) && !File.Exists(DatenbankPfadStandard))
                {
                    Directory.CreateDirectory(appOrdner);
                    File.Copy(alt, DatenbankPfadStandard);
                }
            }
            catch
            {
                // dann wird eben eine neue Datenbank angelegt
            }
        }

        public static void Save()
        {
            try
            {
                string ordner = Path.GetDirectoryName(settingsPfad);
                if (!string.IsNullOrEmpty(ordner))
                {
                    Directory.CreateDirectory(ordner);
                }

                string json = JsonSerializer.Serialize(Aktuell, jsonOptions);
                File.WriteAllText(settingsPfad, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Einstellungen konnten nicht gespeichert werden: {ex.Message}", ex);
            }
        }

        // Zeiten als TimeSpan, bei ungültigem Text in der Datei gilt der Standardwert

        public static TimeSpan ArbeitszeitProTag
        {
            get => TimeSpan.TryParse(Aktuell.ArbeitszeitProTag, out var ts) ? ts : new TimeSpan(8, 40, 0);
            set => Aktuell.ArbeitszeitProTag = value.ToString(@"hh\:mm");
        }

        public static TimeSpan MaxArbeitszeit
        {
            get => TimeSpan.TryParse(Aktuell.MaxArbeitszeit, out var ts) ? ts : new TimeSpan(10, 0, 0);
            set => Aktuell.MaxArbeitszeit = value.ToString(@"hh\:mm");
        }

        public static TimeSpan WarnzeitVorMax
        {
            get => TimeSpan.TryParse(Aktuell.WarnzeitVorMax, out var ts) ? ts : new TimeSpan(9, 30, 0);
            set => Aktuell.WarnzeitVorMax = value.ToString(@"hh\:mm");
        }

        public static bool StartMinimiert
        {
            get => Aktuell.StartMinimiert;
            set => Aktuell.StartMinimiert = value;
        }

        public static int AnzahlTageAnzeige
        {
            get => Aktuell.AnzahlTageAnzeige > 0 ? Aktuell.AnzahlTageAnzeige : 50;
            set => Aktuell.AnzahlTageAnzeige = value;
        }

        // Werte so, wie sie in der Datei stehen (auch leer), für den Einstellungsdialog

        public static string IconPfad
        {
            get => Aktuell.IconPfad ?? "";
            set => Aktuell.IconPfad = value ?? "";
        }

        public static string DatenbankPfad
        {
            get => Aktuell.DatenbankPfad ?? "";
            set => Aktuell.DatenbankPfad = value ?? "";
        }

        private static string IconPfadStandard =>
            Path.Combine(Application.StartupPath, "Resources", "icon_clock.ico");

        private static string DatenbankPfadStandard =>
            Path.Combine(appOrdner, "zeiten.db");

        // eigener Pfad, wenn gesetzt und vorhanden, sonst Standard
        public static string IconPfadEffektiv =>
            !string.IsNullOrWhiteSpace(Aktuell.IconPfad) && File.Exists(Aktuell.IconPfad)
                ? Aktuell.IconPfad
                : IconPfadStandard;

        // eigener Pfad, wenn gesetzt, sonst Standard
        public static string DatenbankPfadEffektiv =>
            !string.IsNullOrWhiteSpace(Aktuell.DatenbankPfad)
                ? Aktuell.DatenbankPfad
                : DatenbankPfadStandard;
    }
}
