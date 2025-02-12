using WatherApplication.Models;

namespace WatherApplication.Services;

public interface IWeatherService
{
    Task<WeatherModel> GetWeatherAsync(string city);
}
