using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class User
	{
		public User()
		{
		}

        [BsonElement("_id")]
        [BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

		public string email { get; set; }

		public string password { get; set; }

        public string? storedsalt { get; set; }
    }
}

