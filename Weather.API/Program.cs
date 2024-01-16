using Microsoft.AspNetCore.Builder;
using Weather.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Access-Control-Allow-Origin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("Access-Control-Allow-Origin");

app.Run();
