using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Weather.Service.Common;
using Weather.Service.Model;
using Weather.Service.Services;

namespace Weather.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _memoryCache;

        public WeatherController(IWeatherService weatherService, IMemoryCache memoryCache)
        {
            _weatherService = weatherService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/Weather")]
        public async Task<IActionResult> GetWeatherAsync(string? location)
        {
            if (location == null)
            {
                var defaultLocation = _memoryCache.Get<string>(Constants.DefaultLocation);
                if (defaultLocation != null)
                {
                    location = defaultLocation;
                }
                else {
                    return NotFound("Location not found");
                }
            }
            
            var weatherCache = _memoryCache.Get<WeatherDetails>(location);

            var weatherDto = new WeatherDetails();

            if (weatherCache == null)
            {
                weatherDto = await _weatherService.GetWeatherAsync(location);

                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.WeatherDetailsCacheInMinutes));

                _memoryCache.Set(location, weatherDto, options);
            }
            return Ok(weatherCache ?? weatherDto);
        }

        [HttpPost]
        [Route("/SetDefaultLocation")]
        public IActionResult SetDefaultLocation([FromBody]Location location)
        {
            if (location.DefautLocation != null)
            {
                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.DefaultLocationCacheInMinutes));

                _memoryCache.Set(Constants.DefaultLocation, location.DefautLocation, options);
                
                return Ok(location);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
