using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoGame.Services;

namespace TodoGame.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GameController : ControllerBase
{
    private readonly IPokeDexService _pokeDexService;

    public GameController(IPokeDexService pokeDexService) {
        _pokeDexService = pokeDexService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult getAllGameUsers()
    {
        return Ok(_pokeDexService.GetPokeDices());
    }

}

