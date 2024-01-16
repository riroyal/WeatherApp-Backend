using Newtonsoft.Json;
using Weather.Service.Model;

namespace Weather.Service.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IAPIService _apiService;

        public WeatherService(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<WeatherDetails> GetWeatherAsync(string location)
        {
            //Get data from weather API
            var responseContent = await _apiService.GetWeatherAsync(location);

            var weather = JsonConvert.DeserializeObject<WeatherModel>(responseContent);

            var weatherDto = new WeatherDetails()
            {
                LocationName = weather.Name,
                Temperature = weather.Main.Temp,
                Humidity = weather.Main.Humidity,
                WindSpeed = weather.Wind.Speed,
                Icon = weather.Weather[0].Icon
            };

            return weatherDto;
        }
    }
}
