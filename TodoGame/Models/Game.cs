using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class Game
	{
		public Game()
		{
		}

        [BsonElement("_id")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int type { get; set; }

        public int status { get; set; } // 0 - Pending 1 - Started 2 - Completed

        public User gamestartuser { get; set; }

        public User? gameopponent { get; set; }
    }
}

