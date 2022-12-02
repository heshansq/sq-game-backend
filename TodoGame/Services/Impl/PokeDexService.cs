using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services.Impl
{
	public class PokeDexService: IPokeDexService
	{

        private readonly MongoDB.Driver.IMongoCollection<PokeDex> _pokeDices;

		public PokeDexService(IDBClient dbClient)
		{
            _pokeDices = dbClient.GetPokeDexCollection();
        }

        public List<PokeDex> GetPokeDices() => _pokeDices.Find(PokeDex => true).ToList();
    }
}

