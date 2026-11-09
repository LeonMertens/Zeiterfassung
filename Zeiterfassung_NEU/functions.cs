using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Zeiterfassung_NEU
{
    internal static class Funktionen
    {
        private static string connectionString;

        /// <summary>
        /// Setzt den Datenbankpfad und legt die Tabelle an, falls nötig.
        /// </summary>
        public static void Init(string dbPath)
        {
            // SQLite legt keine Ordner an, also vorher selbst erstellen
            string ordner = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(ordner))
            {
                Directory.CreateDirectory(ordner);
            }

            connectionString = $"Data Source={dbPath}";
            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"CREATE TABLE IF NOT EXISTS Zeiten (
                                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Datum TEXT NOT NULL,
                                        Ankunft TEXT NOT NULL,
                                        Ende TEXT NOT NULL
                                    )";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Datenbank konnte nicht initialisiert werden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lädt eine Seite der Tabelle. Seite 0 = neueste Einträge.
        /// </summary>
        public static DataTable zeitenVonDBLadenSeite(int seite, int anzahlProSeite)
        {
            try
            {
                DataTable table = new DataTable();
                int offset = Math.Max(0, seite) * Math.Max(1, anzahlProSeite);

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Zeiten ORDER BY id DESC LIMIT @anzahl OFFSET @offset";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@anzahl", anzahlProSeite);
                        command.Parameters.AddWithValue("@offset", offset);
                        using (var reader = command.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Zeiten konnten nicht geladen werden: {ex.Message}", ex);
            }
        }

        public static int anzahlEintraegeGesamt()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Zeiten";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        long count = (long)command.ExecuteScalar();
                        return (int)count;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Anzahl der Einträge konnte nicht ermittelt werden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Neuer Eintrag für heute. Ende = jetzt + Arbeitszeit aus den Einstellungen.
        /// </summary>
        public static (string datum, string ankunft, string ende) createData()
        {
            try
            {
                string date = DateTime.Now.ToString("dd.MM.yyyy");
                string time_arr = DateTime.Now.ToString("HH:mm");
                string time_left = DateTime.Now.Add(EinstellungenVerwaltung.ArbeitszeitProTag).ToString("HH:mm");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Zeiten (Datum, Ankunft, Ende) VALUES (@datum, @ankunft, @ende)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@datum", date);
                        command.Parameters.AddWithValue("@ankunft", time_arr);
                        command.Parameters.AddWithValue("@ende", time_left);
                        command.ExecuteNonQuery();
                    }
                }

                return (date, time_arr, time_left);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Eintrag konnte nicht angelegt werden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Heutiger Eintrag oder null.
        /// </summary>
        public static (string datum, string ankunft, string ende)? heutigenEintragLaden()
        {
            try
            {
                string heute = DateTime.Now.ToString("dd.MM.yyyy");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Datum, Ankunft, Ende FROM Zeiten WHERE Datum = @datum ORDER BY id DESC LIMIT 1";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@datum", heute);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (reader.GetString(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Heutiger Eintrag konnte nicht geladen werden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Wie oben, aber mit ID (wird für "Gehen" gebraucht).
        /// </summary>
        public static (string id, string datum, string ankunft, string ende)? heutigenEintragMitIdLaden()
        {
            try
            {
                string heute = DateTime.Now.ToString("dd.MM.yyyy");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id, Datum, Ankunft, Ende FROM Zeiten WHERE Datum = @datum ORDER BY id DESC LIMIT 1";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@datum", heute);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string id = reader.GetInt64(0).ToString();
                                return (id, reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Heutiger Eintrag (mit ID) konnte nicht geladen werden: {ex.Message}", ex);
            }
        }

        public static void deleteData(string id)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Zeiten WHERE id = @id";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Eintrag konnte nicht gelöscht werden: {ex.Message}", ex);
            }
        }

        public static void saveData(string id, string anfangNeu, string endeNeu)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Zeiten " +
                                   "SET Ankunft = @anfang, Ende = @ende " +
                                   "WHERE id = @id";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@anfang", anfangNeu);
                        command.Parameters.AddWithValue("@ende", endeNeu);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Eintrag konnte nicht gespeichert werden: {ex.Message}", ex);
            }
        }

        public static bool timeTaken()
        {
            try
            {
                string heute = DateTime.Now.ToString("dd.MM.yyyy");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Zeiten WHERE Datum = @datum";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@datum", heute);

                        long count = (long)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Prüfung auf heutigen Eintrag fehlgeschlagen: {ex.Message}", ex);
            }
        }
    }
}