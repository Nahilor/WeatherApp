using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.services
{
    public static class apiservice
    {
        public static async Task<Root> GetWeather(double latitude, double longitude)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}appid=52f786589d25730aa42e7b192eb27719",latitude,longitude));
            return JsonConvert.DeserializeObject<Root>(response);
        }

        public static async Task<Root> GetWeatherByCity(string city)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("api.openweathermap.org/data/2.5/forecast?q={0}&appid=52f786589d25730aa42e7b192eb27719",city));
            return JsonConvert.DeserializeObject<Root>(response);
        }
    }
}
