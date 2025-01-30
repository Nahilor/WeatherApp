using System.Data.SQLite;
using System.Linq.Expressions;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Data
{
    internal class WeatherDAO
    {
        private readonly DatabaseInitializer _databaseInitializer;
        public WeatherDAO(string connectionString) { 
            _databaseInitializer = new DatabaseInitializer(connectionString);
            _databaseInitializer.InitializeDatabase();
        }
       public async Task<WeatherData> GetWeatherDataByCityFromApi(String city) {
            Root r = await WeatherApiService.Get7DayForecastAsync(city, "ec41ae60ff582e38f9d23b71b591d7a0", "forecast");
            var weatherData = new WeatherData(r);

            return weatherData;
       }
        public async Task<WeatherData?> GetWeatherDataFromDatabase(string city)
        {
            using (var connection = new SQLiteConnection(_databaseInitializer._connectionString))
            {
                connection.Open();

                // Check if the city exists in the database
                string query = "SELECT * FROM WeatherTable WHERE City = @City ORDER BY TimeStamp DESC LIMIT 1";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@City", city);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime lastUpdated = reader.GetDateTime(reader.GetOrdinal("TimeStamp"));
                            if ((DateTime.Now - lastUpdated).TotalMinutes <= 5)
                            {
                                // Return existing data if it's still fresh
                                return new WeatherData()
                                {
                                    City = city,
                                    Temp = (int)reader.GetDouble(reader.GetOrdinal("Temp")),
                                    FeelsLike = (int)reader.GetDouble(reader.GetOrdinal("FeelsLike")),
                                    Icon = reader.GetString(reader.GetOrdinal("Icon")),
                                    Cloud = (int)reader.GetDouble(reader.GetOrdinal("Cloud")),
                                    Humidity = (int)reader.GetDouble(reader.GetOrdinal("Humidity")),
                                    Pod = reader.GetString(reader.GetOrdinal("Pod")),
                                    WindSpeed = (int)reader.GetDouble(reader.GetOrdinal("WindSpeed")),
                                    WindDeg = (int)reader.GetDouble(reader.GetOrdinal("WindDeg")),
                                    Pressure = (int)reader.GetDouble(reader.GetOrdinal("Pressure")),
                                    Monday = (int)reader.GetDouble(reader.GetOrdinal("Monday")),
                                    Tuesday = (int)reader.GetDouble(reader.GetOrdinal("Tuesday")),
                                    Wednesday = (int)reader.GetDouble(reader.GetOrdinal("Wednesday")),
                                    Thursday = (int)reader.GetDouble(reader.GetOrdinal("Thursday")),
                                    Friday = (int)reader.GetDouble(reader.GetOrdinal("Friday")),
                                    Saturday = (int)reader.GetDouble(reader.GetOrdinal("Saturday")),
                                    Sunday = (int)reader.GetDouble(reader.GetOrdinal("Sunday")),
                                    TimeStamp = lastUpdated
                                };
                            }
                        }
                    }
                }
            }

            WeatherData? weatherData = null;
            try
            {
                // If data is outdated or city doesn't exist, fetch from API
                weatherData = await GetWeatherDataByCityFromApi(city);
                if (weatherData != null)
                {
                    // Save the new data to the database
                    SaveWeatherDataToDatabase(weatherData);
                }
            }
            catch(HttpRequestException e) 
            {
                MessageBox.Show("Data couldn't be updated because of API access issues. Turn on internet access!!\n Note: if available previous or older version information will be displayed.");
                // return if the api couldn't get any data or there is no internet
                using (var connection = new SQLiteConnection(_databaseInitializer._connectionString))
                {
                    connection.Open();

                    // Check if the city exists in the database
                    string query = "SELECT * FROM WeatherTable WHERE City = @City ORDER BY TimeStamp DESC LIMIT 1";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@City", city);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime lastUpdated = reader.GetDateTime(reader.GetOrdinal("TimeStamp"));
                                weatherData = new WeatherData()
                                {
                                    City = city,
                                    Temp = (int)reader.GetDouble(reader.GetOrdinal("Temp")),
                                    FeelsLike = (int)reader.GetDouble(reader.GetOrdinal("FeelsLike")),
                                    Icon = reader.GetString(reader.GetOrdinal("Icon")),
                                    Cloud = (int)reader.GetDouble(reader.GetOrdinal("Cloud")),
                                    Humidity = (int)reader.GetDouble(reader.GetOrdinal("Humidity")),
                                    Pod = reader.GetString(reader.GetOrdinal("Pod")),
                                    WindSpeed = (int)reader.GetDouble(reader.GetOrdinal("WindSpeed")),
                                    WindDeg = (int)reader.GetDouble(reader.GetOrdinal("WindDeg")),
                                    Pressure = (int)reader.GetDouble(reader.GetOrdinal("Pressure")),
                                    Monday = (int)reader.GetDouble(reader.GetOrdinal("Monday")),
                                    Tuesday = (int)reader.GetDouble(reader.GetOrdinal("Tuesday")),
                                    Wednesday = (int)reader.GetDouble(reader.GetOrdinal("Wednesday")),
                                    Thursday = (int)reader.GetDouble(reader.GetOrdinal("Thursday")),
                                    Friday = (int)reader.GetDouble(reader.GetOrdinal("Friday")),
                                    Saturday = (int)reader.GetDouble(reader.GetOrdinal("Saturday")),
                                    Sunday = (int)reader.GetDouble(reader.GetOrdinal("Sunday")),
                                    TimeStamp = lastUpdated
                                };
                                return weatherData;
                            }
                        }
                    }
                }
            }

            if (weatherData == null)
            {
                MessageBox.Show($"City Doesn't have existing data please connect to the internet.");
            }

            return weatherData;
        }

        private void SaveWeatherDataToDatabase(WeatherData weather)
        {
            weather.TimeStamp = DateTime.Now;
            using (var connection = new SQLiteConnection(_databaseInitializer._connectionString))
            {
                connection.Open();
                string query = @"
            INSERT INTO WeatherTable (Id, City, TimeStamp, Temp, FeelsLike, Icon, Cloud,Humidity, Pod, WindSpeed, WindDeg, Pressure, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday)
            VALUES (@Id, @City, @TimeStamp, @Temp, @FeelsLike, @Icon, @Cloud, @Humidity, @Pod, @WindSpeed, @WindDeg, @Pressure, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)
            ON CONFLICT(Id) DO UPDATE SET
                TimeStamp = excluded.TimeStamp,
                Temp = excluded.Temp,
                FeelsLike = excluded.FeelsLike,
                Icon = excluded.Icon,
                Cloud = excluded.Cloud,
                Humidity = excluded.Humidity,
                Pod = excluded.Pod,
                WindSpeed = excluded.WindSpeed,
                WindDeg = excluded.WindDeg,
                Pressure = excluded.Pressure,
                Monday = excluded.Monday,
                Tuesday = excluded.Tuesday,
                Wednesday = excluded.Wednesday,
                Thursday = excluded.Thursday,
                Friday = excluded.Friday,
                Saturday = excluded.saturday,
                Sunday = excluded.Sunday";


                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", weather.Id);
                    command.Parameters.AddWithValue("@City", weather.City);
                    command.Parameters.AddWithValue("@TimeStamp", weather.TimeStamp);
                    command.Parameters.AddWithValue("@Temp", weather.Temp);
                    command.Parameters.AddWithValue("@FeelsLike", weather.FeelsLike);
                    command.Parameters.AddWithValue("@Icon", weather.Icon);
                    command.Parameters.AddWithValue("@Cloud", weather.Cloud);
                    command.Parameters.AddWithValue("@Humidity", weather.Humidity);
                    command.Parameters.AddWithValue("@Pod", weather.Pod);
                    command.Parameters.AddWithValue("@WindSpeed", weather.WindSpeed);
                    command.Parameters.AddWithValue("@WindDeg", weather.WindDeg);
                    command.Parameters.AddWithValue("@Pressure", weather.Pressure);
                    command.Parameters.AddWithValue("@Monday", weather.Monday);
                    command.Parameters.AddWithValue("@Tuesday", weather.Tuesday);
                    command.Parameters.AddWithValue("@Wednesday", weather.Wednesday);
                    command.Parameters.AddWithValue("@Thursday", weather.Thursday);
                    command.Parameters.AddWithValue("@Friday", weather.Friday);
                    command.Parameters.AddWithValue("@Saturday", weather.Saturday);
                    command.Parameters.AddWithValue("@Sunday", weather.Sunday);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
