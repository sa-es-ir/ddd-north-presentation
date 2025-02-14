using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultiAuth.ApiService.Controllers;

[ApiController]
[Route("api")]
public class ApiBoxController : ControllerBase
{
    [HttpGet("server-a")]
    [Authorize(Roles = "Role-A")]
    public IEnumerable<WeatherForecast> GetServerA()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries.List[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("server-b")]
    [Authorize(Roles = "Role-B")]
    public IEnumerable<WeatherForecast> GetServerB()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries.List[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("custom")]
    [Authorize]
    public IEnumerable<WeatherForecast> GetCustomToken()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries.List[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
