using System;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface IGameService
	{
        Game createGame(Game game);
        List<Game> listUserGame(string userId);
    }
}

