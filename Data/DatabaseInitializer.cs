using System;
using System.Data.SQLite;

namespace WeatherApp.Data
{
    public class DatabaseInitializer
    {
        public string _connectionString {  get; set; }

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }



        public void InitializeDatabase()
        {
            if (!System.IO.File.Exists("Weather.sqlite"))
            {
                SQLiteConnection.CreateFile("Weather.sqlite");
            }

            // Initializing the database schema
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS WeatherTable (
                    Id INTEGER PRIMARY KEY,
                    City TEXT NOT NULL UNIQUE,
                    TimeStamp TEXT NOT NULL,
                    Temp REAL NOT NULL,
                    FeelsLike REAL NOT NULL,
                    Icon TEXT NOT NULL,
                    Cloud REAL NOT NULL,
                    Humidity REAL NOT NULL,
                    Pod Text NOT NULL,
                    WindSpeed REAL NOT NULL,  
                    WindDeg REAL NOT NULL,
                    Pressure REAL NOT NULL, 
                    Monday REAL NOT NULL,
                    Tuesday REAL NOT NULL,
                    Wednesday REAL NOT NULL,
                    Thursday REAL NOT NULL,
                    Friday REAL NOT NULL,
                    Saturday REAL NOT NULL,  
                    Sunday REAL NOT NULL
                );
                ";
                command.ExecuteNonQuery();
            }
        }
    }
}
