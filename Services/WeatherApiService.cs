using System.Net.Http;
using System.Text.Json;
using System.DirectoryServices.ActiveDirectory;
using WeatherApp.Models;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace WeatherApp.Services
{
    public class WeatherApiService
    {
        private static readonly HttpClient? _httpClient = new HttpClient();
        public static string json;
        public static async Task<Root> Get7DayForecastAsync(string city, string apiKey, string endPoint)
        {
            // Make the HTTP GET request
            try
            {
                using HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) };
                HttpResponseMessage response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/{endPoint}/?q={city}&cnt=7&appid={apiKey}");
                json = await response.Content.ReadAsStringAsync();
            }
            catch (TaskCanceledException e)
            {
                MessageBox.Show($"Request timed out: {e.Message}");
            }
            //MessageBox.Show(json);
            return JsonSerializer.Deserialize<Root>(json);
        }
    }
}

