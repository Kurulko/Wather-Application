using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WatherApplication.Models;
using WatherApplication.Services;

namespace WatherApplication.Controllers;

public class HomeController : Controller
{
    readonly IWeatherService weatherService;
    readonly IHttpContextAccessor httpContextAccessor;
    public HomeController(IWeatherService weatherService, IHttpContextAccessor httpContextAccessor)
    {
        this.weatherService = weatherService;
        this.httpContextAccessor = httpContextAccessor;
    }

    const string lastCitySessionName = "LastCity";
    const string defaultCityName = "Kyiv";
    ISession Session => httpContextAccessor.HttpContext!.Session;

    public async Task<IActionResult> Index(string city)
    {
        try
        {
            if(string.IsNullOrEmpty(city))
                city = Session.GetString(lastCitySessionName) ?? defaultCityName;

            var weather = await weatherService.GetWeatherAsync(city);
            Session.SetString(lastCitySessionName, city);

            string warnedCitySessionName = getWarnedCitySessionName(city);
            string todayShortUTCStr = DateTime.UtcNow.ToShortDateString();

            bool isWarnedToday = Session.GetString(warnedCitySessionName) == todayShortUTCStr;
            if (weather.HasPrecipitation && !isWarnedToday)
            {
                ViewBag.Warning = $"Warning: Rain is expected in {city} today!";
                Session.SetString(warnedCitySessionName, todayShortUTCStr);
            }

            return View(weather);
        }
        catch (HttpRequestException)
        {
            ViewBag.Error = "Could not retrieve weather data. Please try again later.";
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
        }

        return View();
    }

    string getWarnedCitySessionName(string city)
        => $"Warned_{city}";
}