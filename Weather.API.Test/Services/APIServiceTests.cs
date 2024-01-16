using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Weather.Service.Services;
using Microsoft.Extensions.Configuration;
using Moq.Protected;

namespace Weather.API.Test.Services
{
    [TestFixture]
    public class APIServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<ILogger<APIService>> _loggerMock;
        private Mock<IConfiguration> _configurationMock;
        private string _responseJson;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _loggerMock = new Mock<ILogger<APIService>>();
            _configurationMock = new Mock<IConfiguration>();
        }

        [Test]
        public async Task GetWeatherAsync_SuccessfulRequest_ReturnsApiResponse()
        {
            // Arrange
            var location = "London";
            var baseUrl = "http://api.openweathermap.org";
            var endPoint = "data/2.5/weather?q={location}&appid={apikey}&units=metric";
            var apiKey = "8ef961e47590304d8e701699df4dde83";

            _configurationMock.Setup(config => config.GetSection(Service.Common.Constants.Settings).GetSection(Service.Common.Constants.BaseUrl).Value)
                .Returns(baseUrl);
            _configurationMock.Setup(config => config.GetSection(Service.Common.Constants.Settings).GetSection(Service.Common.Constants.WeatherEndPoint).Value)
                .Returns(endPoint);
            _configurationMock.Setup(config => config.GetSection(Service.Common.Constants.Settings).GetSection(Service.Common.Constants.WeatherApiKey).Value)
                .Returns(apiKey);

            _responseJson = Utils.GetJsonFromTestDataFile();

            var apiService = new APIService(new HttpClient(_httpMessageHandlerMock.Object), _loggerMock.Object, _configurationMock.Object);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(_responseJson)
                });

            // Act
            var result = await apiService.GetWeatherAsync(location);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(_responseJson));
        }
    }
}
