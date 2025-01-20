using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public static class ApiService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<Root> GetWeather(double latitude, double longitude)
        {
            try
            {
                var response = await httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid=52f786589d25730aa42e7b192eb27719");
                return JsonConvert.DeserializeObject<Root>(response);
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine("Error fetching weather data: " + httpRequestException.Message);
                return null;
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occurred: " + exception.Message);
                return null;
            }
        }

        public static async Task<Root> GetWeatherByCity(string city)
        {
            try
            {
                var response = await httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid=52f786589d25730aa42e7b192eb27719");
                return JsonConvert.DeserializeObject<Root>(response);
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine("Error fetching weather data: " + httpRequestException.Message);
                return null;
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occurred: " + exception.Message);
                return null;
            }
        }
    }
}
