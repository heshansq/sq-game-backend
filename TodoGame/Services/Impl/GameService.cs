using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services.Impl
{
	public class GameService : IGameService
	{
        public readonly IMongoCollection<Game> _games;
        public readonly IMongoCollection<User> _users;
        private readonly ISignalRMessageService _signalRMessageService;
        private readonly IUserService _userService;

        public GameService(IDBClient dBClient, ISignalRMessageService signalRMessageService, IUserService userService)
		{
            _games = dBClient.GetGameCollection();
            _users = dBClient.GetUserCollection();
            _signalRMessageService = signalRMessageService;
            _userService = userService;

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

        public UpdateResult updateGameStatus(string gameId, int gameStatus)
        {
            var filter = Builders<Game>.Filter.Eq("Id", gameId);
            var update = Builders<Game>.Update.Set("status", gameStatus);

            return _games.UpdateOne(filter, update);
        }

        public UpdateResult updateOpponent(string gameId, string opId)
        {
            var filter = Builders<Game>.Filter.Eq("Id", gameId);
            User opUser = _userService.GetUser(opId);
            var update = Builders<Game>.Update.Set("gameopponent", opUser);

            return _games.UpdateOne(filter, update);
        }

        public Game getGameById(string gameId) => _games.Find<Game>(game => game.Id == gameId).FirstOrDefault();

        async public Task gameWinningCheck(string gameId)
        {
            Thread.Sleep(2000);
            Game game = this.getGameById(gameId);

            var playersArr = new string[] { game.gamestartuser.Id.ToString(), game.gameopponent.Id.ToString() };
            Random random = new Random();
            int winnerIndex = random.Next(0, playersArr.Length);

            string winnerId = playersArr[winnerIndex];

            User winnerUser = _userService.GetUser(winnerId);


            SignalRMessage message = new SignalRMessage();
            message.messageType = "gameWon";
            message.startuserid = game.gamestartuser.connectionid.ToString();
            message.opuserid = game.gameopponent.connectionid.ToString();
            message.message = "gameWon";
            message.messagetype = 2;
            message.opuserpublickey = game.gameopponent.publickey;
            message.startuserpublickey = game.gamestartuser.publickey;
            message.winnerUser = winnerUser;
            _signalRMessageService.sendGameStatusNotificationAsync(message);
        }

        public void startWinningCheck(string gameId)
        {
            Thread.Sleep(2000);
            Game game = this.getGameById(gameId);

            var playersArr = new string[] { game.gamestartuser.Id.ToString(), game.gameopponent.Id.ToString() };
            Random random = new Random();
            int winnerIndex = random.Next(0, playersArr.Length);

            string winnerId = playersArr[winnerIndex];

            User winnerUser = _userService.GetUser(winnerId);


            SignalRMessage message = new SignalRMessage();
            message.messageType = "gameWon";
            message.startuserid = game.gamestartuser.connectionid.ToString();
            message.opuserid = game.gameopponent.connectionid.ToString();
            message.message = "gameWon";
            message.messagetype = 2;
            message.opuserpublickey = game.gameopponent.publickey;
            message.startuserpublickey = game.gamestartuser.publickey;
            message.winnerUser = winnerUser;
            _signalRMessageService.sendGameStatusNotificationAsync(message);
        }
    }
}

