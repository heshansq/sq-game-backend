using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
	public class User
	{
		public User()
		{
		}

		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
    }
}

