using Moq;
using Weather.Service.Services;

namespace Weather.API.Test.Services
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private Mock<IAPIService> _mockApiService;

        [SetUp]
        public void SetUp()
        {
            _mockApiService = new Mock<IAPIService>();
        }

        [Test]
        public async Task GetWeatherAsync_WeatherData_ReturnsObjectNotNull()
        {
            //Arrange
            var location = "London";
            var weatherDetailsJsonString = "{\"coord\":{\"lon\":-0.1257,\"lat\":51.5085},\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"clear sky\",\"icon\":\"01d\"}],\"base\":\"stations\",\"main\":{\"temp\":-0.86,\"feels_like\":-5.43,\"temp_min\":-2.22,\"temp_max\":0.2,\"pressure\":1011,\"humidity\":85},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":270},\"clouds\":{\"all\":2},\"dt\":1705309144,\"sys\":{\"type\":2,\"id\":2091269,\"country\":\"GB\",\"sunrise\":1705305593,\"sunset\":1705335542},\"timezone\":0,\"id\":2643743,\"name\":\"London\",\"cod\":200}";

            _mockApiService.Setup(x =>
                   x.GetWeatherAsync(location))
               .ReturnsAsync(weatherDetailsJsonString);

            var weatherService = new WeatherService(_mockApiService.Object);

            //Act
            var repsonse = await weatherService.GetWeatherAsync("London");
            
            //Assert
            Assert.That(repsonse, Is.Not.Null);
            Assert.That(repsonse.LocationName, Is.EqualTo(location));
        }
    }
}
