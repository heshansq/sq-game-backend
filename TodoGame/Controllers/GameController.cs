using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoGame.Models;
using TodoGame.Services;

namespace TodoGame.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize]
public class GameController : ControllerBase
{
    private readonly IPokeDexService _pokeDexService;
    private readonly IUserService _userService;
    private readonly IGameService _gameService;

    public GameController(IPokeDexService pokeDexService, IUserService userService, IGameService gameService) {
        _pokeDexService = pokeDexService;
        _userService = userService;
        _gameService = gameService;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("getpokedexes")]
    public IActionResult getAllPokeDices()
    {
        return Ok(_pokeDexService.GetPokeDices());
    }

    [HttpGet("{userid}")]
    public ActionResult<List<Game>> getUserGameList(string userid)
    {
        return _gameService.listUserGame(userid);
    }

    [HttpPost]
    public ActionResult<Game> createNewGame(GameDto gameData)
    {
        Game gameSt = new Game();
        gameSt.type = gameData.type;
        gameSt.status = gameData.status;

        User gameStartUser = _userService.GetUser(gameData.gamestartuser);
        gameSt.gamestartuser = gameStartUser;

        Game saveGame = _gameService.createGame(gameSt);

        return saveGame;
    }
}

