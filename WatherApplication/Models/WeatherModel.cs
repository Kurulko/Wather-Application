namespace WatherApplication.Models;

public class WeatherModel
{
    public string City { get; set; } = null!;
    public bool HasPrecipitation { get; set; }
    public Metric Temperature { get; set; } = null!;
    public Metric TemperatureFeelsLike { get; set; } = null!;
    public Metric MinTemperature { get; set; } = null!;
    public Metric MaxTemperature { get; set; } = null!;
    public Metric Precipitation { get; set; } = null!;
}
