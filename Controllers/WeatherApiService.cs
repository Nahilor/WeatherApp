using System.Net.Http;
using System.Text.Json;
using System.DirectoryServices.ActiveDirectory;
using WeatherApp.Models;
using System.ComponentModel;
using System.Net.Http.Headers;

public class WeatherApiService
{
    private static readonly HttpClient? _httpClient = new HttpClient();
    public static string json;
    public static async Task<Root> Get7DayForecastAsync(string city, string apiKey, string endPoint)
    {
        // Make the HTTP GET request
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/{endPoint}/?q={city}&cnt=7&appid={apiKey}");
            json = await response.Content.ReadAsStringAsync();
            //MessageBox.Show(json);
        }


        return JsonSerializer.Deserialize<Root>(json);
    }
}

