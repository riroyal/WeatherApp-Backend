using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Weather.API.Controllers;
using Weather.Service.Model;
using Weather.Service.Services;

namespace Weather.API.Test.Controllers
{
    [TestFixture]
    public class WeatherControllerTests
    {
        private Mock<IWeatherService> _mockWeatherService;
        private Mock<IMemoryCache> _mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockMemoryCache = new Mock<IMemoryCache>();
        }

        [Test]
        public async Task GetWeatherAsync_WithValidLocation_ReturnsOkResult()
        {
            // Arrange
            var location = "London";
            var weatherDetails = new WeatherDetails 
            {
                LocationName = "London",
                Temperature = 3,
                Humidity = 85,
                WindSpeed = 40,
                Icon = "01n"
            };

            _mockWeatherService.Setup(x =>
                    x.GetWeatherAsync(location))
                .ReturnsAsync(weatherDetails);

            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var controller = new WeatherController(_mockWeatherService.Object, _mockMemoryCache.Object);

            // Act
            var result = await controller.GetWeatherAsync(location) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(weatherDetails));
        }
    }
}
