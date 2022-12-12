using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface IDBClient
	{
        IMongoCollection<PokeDex> GetPokeDexCollection();
		IMongoCollection<User> GetUserCollection();
		IMongoCollection<Todo> GetTodoCollection();
		IMongoCollection<Game> GetGameCollection();
    }
}

