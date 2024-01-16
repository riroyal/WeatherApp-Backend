namespace Weather.Service.Model
{
    public record WeatherDetails
    {
        public string LocationName { get; init; }
        public double Temperature { get; init; }
        public long Humidity { get; init; }
        public double WindSpeed { get; init; }
        public string? Icon { get; init; }
    }
}
