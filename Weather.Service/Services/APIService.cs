using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Weather.Service.Common;

namespace Weather.Service.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<APIService> _logger;
        private readonly IConfiguration _configuration;

        public APIService(HttpClient httpClient, ILogger<APIService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = config;
        }

        public async Task<string?> GetWeatherAsync(string location)
        {
            try
            {
                var baseUrl = _configuration.GetSection(Constants.Settings).GetSection(Constants.BaseUrl).Value;
                var endPoint = _configuration.GetSection(Constants.Settings).GetSection(Constants.WeatherEndPoint).Value;
                var weatherApiKey = _configuration.GetSection(Constants.Settings).GetSection(Constants.WeatherApiKey).Value;

                endPoint = endPoint.Replace(Constants.LocationPlaceHolder, location).Replace(Constants.ApiKeyPlaceHolder, weatherApiKey);

                _httpClient.BaseAddress = new Uri(baseUrl);

                var apiResponse = await _httpClient.GetAsync(endPoint);

                apiResponse.EnsureSuccessStatusCode();

                return await apiResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error occurred while reading Weather api {ex.Message}");
                throw;
            }
        }
    }
}
