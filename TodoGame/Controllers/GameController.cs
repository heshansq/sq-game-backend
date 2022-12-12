using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
    public IActionResult createNewGame(GameDto gameData)
    {
        Game gameSt = new Game();
        gameSt.type = gameData.type;
        gameSt.status = gameData.status;

        User gameStartUser = _userService.GetUser(gameData.gamestartuser);
        gameSt.gamestartuser = gameStartUser;

        if (gameStartUser.tickets == null || gameStartUser.tickets == 0)
        {
            return new ObjectResult("User dont have tickets to start a game!") { StatusCode = 403 };
        }

        if (gameData.gameopponent != null)
        {
            User gameOpUser = _userService.GetUser(gameData.gameopponent);
            gameSt.gameopponent = gameOpUser;
        }

        var ticketAmt = gameStartUser.tickets != 0 ? gameStartUser.tickets - 1 : 0;
        UpdateResult userUpdate = _gameService.changeTickets(gameData.gamestartuser, ticketAmt);

        Game saveGame = _gameService.createGame(gameSt);

        return Ok(saveGame);
    }
}

