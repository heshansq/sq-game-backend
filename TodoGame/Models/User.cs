using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class User
	{
		public User()
		{
		}

		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }

		public string email { get; set; }

		public string password { get; set; }
    }
}

