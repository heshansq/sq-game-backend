using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services
{
	public class DBClient: IDBClient
	{
        private readonly IMongoCollection<PokeDex> _pokeDices;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Todo> _todos;
        private readonly IMongoCollection<Game> _games;

        public DBClient(IOptions<Config.DBConfig> dbConfig)
		{
            var client = new MongoClient(dbConfig.Value.Database_Connection_String);
            var database = client.GetDatabase(dbConfig.Value.Database_Name);

            var dtt = database.GetCollection<PokeDex>(dbConfig.Value.Poke_Dex_Collection);

            _pokeDices = dtt;
            _users = database.GetCollection<User>(dbConfig.Value.User_Collection);
            _todos = database.GetCollection<Todo>(dbConfig.Value.Todo_Collection);
            _games = database.GetCollection<Game>(dbConfig.Value.Game_Collection);

        }

        public IMongoCollection<PokeDex> GetPokeDexCollection() => _pokeDices;

        public IMongoCollection<Todo> GetTodoCollection() => _todos;

        public IMongoCollection<User> GetUserCollection() => _users;

        public IMongoCollection<Game> GetGameCollection() => _games;
    }
}

