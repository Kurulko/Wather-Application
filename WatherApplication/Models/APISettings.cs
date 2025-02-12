namespace WatherApplication.Models;

public class APISettings
{
    public string WeatherApiUrl { get; set; } = null!;
    public string LocationApiUrl { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}
