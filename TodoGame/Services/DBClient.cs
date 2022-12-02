using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services
{
	public class DBClient: IDBClient
	{
        private readonly IMongoCollection<PokeDex> _pokeDices;

        public DBClient(Microsoft.Extensions.Options.IOptions<Config.DBConfig> dbConfig)
		{
            var client = new MongoClient(dbConfig.Value.Database_Connection_String);
            var database = client.GetDatabase(dbConfig.Value.Database_Name);

            _pokeDices = database.GetCollection<PokeDex>(dbConfig.Value.Poke_Dex_Collection);
		}

        public IMongoCollection<PokeDex> GetPokeDexCollection() => _pokeDices;
    }
}

