using Microsoft.AspNetCore.Mvc;
using TodoGame.Services;

namespace TodoGame.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IPokeDexService _pokeDexService;

    public GameController(IPokeDexService pokeDexService) {
        _pokeDexService = pokeDexService;
    }

    [HttpGet]
    public IActionResult getAllGameUsers()
    {
        return Ok(_pokeDexService.GetPokeDices());
    }

    /*

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

     */

}

