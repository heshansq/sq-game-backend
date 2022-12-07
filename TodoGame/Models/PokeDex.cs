using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
	public class PokeDex
	{
	
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? _id { get; set; }

		public string? id { get; set; }

        public string? name { get; set; }

        public string? description { get; set; }

        public string? desc { get; set; }

        public string? species { get; set; }

        public string? evolution { get; set; }

        public int? stage { get; set; }

        public int? evolveAt { get; set; }

        public string[]? types { get; set; }

        //public object[]? evYield { get; set; }

        public int? exp { get; set; }

        public Move? move { get; set; }

        public StatsObject? evYield { get; set; }

        public StatsObject? baseStats { get; set; }

        public int? expYield { get; set; }
    }
}

