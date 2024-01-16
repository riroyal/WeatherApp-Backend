namespace Weather.Service.Services
{
    public interface IAPIService
    {
        Task<string?> GetWeatherAsync(string location);
    }
}