using Microsoft.Extensions.Configuration;
using WatherApplication.Models;
using WatherApplication.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();

var jwtSettings = builder.Configuration.GetSection("APISettings").Get<APISettings>()!;
services.AddSingleton(jwtSettings);

services.AddHttpClient<IWeatherService, WeatherService>();

services.AddHttpContextAccessor();
services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
