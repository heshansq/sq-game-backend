using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services.Impl
{
	public class GameService : IGameService
	{
        public readonly IMongoCollection<Game> _games;
        public readonly IMongoCollection<User> _users;

		public GameService(IDBClient dBClient)
		{
            _games = dBClient.GetGameCollection();
            _users = dBClient.GetUserCollection();

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

        public UpdateResult changeTickets(string userid, int? ticketAmt)
        {
            var filter = Builders<User>.Filter.Eq("Id", userid);
            var update = Builders<User>.Update.Set("tickets", ticketAmt);

            return _users.UpdateOne(filter, update);
        }

        public UpdateResult changeTickets(string userid, int ticketAmt)
        {
            var filter = Builders<User>.Filter.Eq("Id", userid);
            var update = Builders<User>.Update.Set("tickets", ticketAmt);

            return _users.UpdateOne(filter, update);
        }
    }
}

