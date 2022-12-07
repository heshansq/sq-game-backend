using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class StatusChanges
	{
        public string? target { get; set; }
        public double? burn { get; set; }
        public string? freeze { get; set; }
        public string? paralysis { get; set; }
        public double? poison { get; set; }
        public double? sleep { get; set; }
        public string? confusion { get; set; }
        public double? flinch { get; set; }
        public int? leechSeed { get; set; }
        public bool? safeguard { get; set; }
        public string? defensiveCurl { get; set; }
        public int? fireSpin { get; set; }
        public string? waterSport { get; set; }
        public string? wrap { get; set; }
        public string? sandstorm { get; set; }
        public bool? rain { get; set; }
        public bool? boolean { get; set; }
    }
}

