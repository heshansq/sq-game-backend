using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class StatsObject
	{
		public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public int power { get; set; }
        public int resist { get; set; }
        public int speed { get; set; }
    }
}

