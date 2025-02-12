using WatherApplication.Models;

namespace WatherApplication.Services;

public class WeatherService : IWeatherService
{
    readonly HttpClient httpClient;
    readonly APISettings apiSettings;

    public WeatherService(HttpClient httpClient, APISettings apiSettings)
    {
        this.httpClient = httpClient;
        this.apiSettings = apiSettings;
    }

    public async Task<WeatherModel> GetWeatherAsync(string city)
    {
        string locationUrl = $"{apiSettings.LocationApiUrl}?apikey={apiSettings.ApiKey}&q={city}";
        var locations = await httpClient.GetFromJsonAsync<AccuWeatherLocation[]>(locationUrl);
        var locationKey = locations?.FirstOrDefault()?.Key;

        if (string.IsNullOrEmpty(locationKey))
            throw new Exception("Location not found.");

        var currentWeather = (await httpClient.GetFromJsonAsync<AccuWeatherCurrentConditions[]>(
            $"{apiSettings.WeatherApiUrl}/{locationKey}?apikey={apiSettings.ApiKey}&metric=true&details=true"
        ))!.First();

        return new WeatherModel
        {
            City = city,
            HasPrecipitation = currentWeather.HasPrecipitation,
            Temperature = currentWeather.Temperature.Metric,
            MinTemperature = currentWeather.TemperatureSummary.Past12HourRange.Minimum.Metric,
            MaxTemperature = currentWeather.TemperatureSummary.Past12HourRange.Maximum.Metric,
            Precipitation = currentWeather.PrecipitationSummary.Precipitation.Metric
        };
    }
}

public class AccuWeatherCurrentConditions
{
    public bool HasPrecipitation { get; set; }
    public Temperature Temperature { get; set; } = null!;
    public TemperatureSummary TemperatureSummary { get; set; } = null!;
    public PrecipitationSummary PrecipitationSummary { get; set; } = null!;
}

public class Temperature
{
    public Metric Metric { get; set; } = null!;
}

public class TemperatureSummary
{
    public Past12HourRange Past12HourRange { get; set; } = null!;
}

public class Maximum
{
    public Metric Metric { get; set; } = null!;
}

public class Minimum
{
    public Metric Metric { get; set; } = null!;
}

public class Past12HourRange
{
    public Minimum Minimum { get; set; } = null!;
    public Maximum Maximum { get; set; } = null!;
}

public class Precipitation
{
    public Metric Metric { get; set; } = null!;
}

public class PrecipitationSummary
{
    public Precipitation Precipitation { get; set; } = null!;
}

public class AccuWeatherLocation
{
    public string Key { get; set; } = null!;
}