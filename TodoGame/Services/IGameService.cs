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
        UpdateResult updateOpponent(string gameId, string opId);
        Game getGameById(string gameId);
        Task gameWinningCheck(string gameId);
        void startWinningCheck(string gameId);
    }
}

