using Weather.Service.Model;

namespace Weather.Service.Services
{
    public interface IWeatherService
    {
        Task<WeatherDetails> GetWeatherAsync(string location);
    }
}