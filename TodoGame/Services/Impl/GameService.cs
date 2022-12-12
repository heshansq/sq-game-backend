using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services.Impl
{
	public class GameService : IGameService
	{
        public readonly IMongoCollection<Game> _games;

		public GameService(IDBClient dBClient)
		{
            _games = dBClient.GetGameCollection();

        }

        public Game createGame(Game game)
        {
            _games.InsertOne(game);
            return game;
        }

        public List<Game> listUserGame(string userId)
        {
            return _games.Find(game => game.gamestartuser.Id == userId).ToList();
        }
    }
}

