using System;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            double latitude = 9.03;  // Latitude for Addis Ababa
            double longitude = 38.74; // Longitude for Addis Ababa

            var weatherData = await ApiService.GetWeather(latitude, longitude);
            if (weatherData != null)
            {
                DisplayWeather(weatherData);
            }
            else
            {
                Console.WriteLine("Failed to retrieve weather data.");
            }
        }

        static void DisplayWeather(Root weatherData)
        {
            Console.WriteLine($"City: {weatherData.city.name}");
            Console.WriteLine($"Temperature: {weatherData.list[0].main.temp}°C");
            Console.WriteLine($"Weather: {weatherData.list[0].weather[0].description}");
        }
    }
}
