using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultiAuth.ApiService.Controllers;

[ApiController]
[Route("api")]
public class ApiBoxController : ControllerBase
{
    [HttpGet("azure-b2c")]
    [Authorize(Roles = "role-azure-b2c")]
    public IEnumerable<WeatherForecast> GetByAzureB2C()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries.List[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("okta")]
    [Authorize(Roles = "role-okta")]
    public IEnumerable<WeatherForecast> GetByOkta()
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
    public IEnumerable<WeatherForecast> GetByCustomToken()
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
