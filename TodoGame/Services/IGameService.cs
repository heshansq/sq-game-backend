using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface IGameService
	{
        Game createGame(Game game);
        List<Game> listUserGame(string userId);
        UpdateResult changeTickets(string userid, int ticketAmt);
        UpdateResult changeTickets(string gamestartuser, int? ticketAmt);
        UpdateResult updateGameStatus(string gameId, int gameStatus);
        Game getGameById(string gameId);
    }
}

