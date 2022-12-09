using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class Todo
	{
		public Todo()
		{
		}

        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string title { get; set; }
        public string? desc { get; set; }
        public int priority { get; set; } //0 - Small, 1 - Medium, 2 - Critical
        public User? aluser { get; set; }
        public int status { get; set; } //0 - Pending 1 - in Progress 2 - Completed
    }
}

