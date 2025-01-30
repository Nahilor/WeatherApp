using WeatherApp.Models;

namespace WeatherApp.Models
{
    internal class WeatherData
    {
        public WeatherData() { }
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Temp { get; set; }
        public int FeelsLike { get; set; }
        public string Icon { get; set; }
        public int Cloud { get; set; }
        public int Humidity { get; set; }
        public String Pod { get; set; }
        public int WindSpeed { get; set; }
        public int WindDeg { get; set; }
        public int Pressure { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; set; }

        public WeatherData(Root root)
        {
            Id = root.city.id;
            City = root.city.name;
            TimeStamp = DateTime.ParseExact(root.list[0].dt_txt, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            Temp = Convert.ToInt32(root.list[0].main.temp);
            FeelsLike = Convert.ToInt32(root.list[0].main.feels_like);
            Icon = root.list[0].weather[0].icon;
            Temp = Convert.ToInt32(root.list[0].main.temp);
            Cloud = Convert.ToInt32(root.list[0].clouds.all);
            Humidity = Convert.ToInt32(root.list[0].main.humidity);
            Pod = root.list[0].sys.pod;
            Pressure = Convert.ToInt32(root.list[0].main.pressure);
            WindSpeed = Convert.ToInt32(root.list[0].wind.speed);
            WindDeg = Convert.ToInt32(root.list[0].wind.deg);
            WindSpeed = Convert.ToInt32(root.list[0].wind.speed);
            Monday = Convert.ToInt32(root.list[0].main.temp);
            Tuesday = Convert.ToInt32(root.list[1].main.temp);
            Wednesday = Convert.ToInt32(root.list[2].main.temp);
            Thursday = Convert.ToInt32(root.list[3].main.temp);
            Friday = Convert.ToInt32(root.list[4].main.temp);
            Saturday = Convert.ToInt32(root.list[5].main.temp);
            Sunday = Convert.ToInt32(root.list[6].main.temp);
        }

    }
}
