using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class StatChanges
	{
        public string? target { get; set; }

        public int? speed { get; set; }

        //public double? chance { get; set; }

        public int? defense { get; set; }

        public int? attack { get; set; }

        public int? resist { get; set; }

        public int? accuracy { get; set; }

        public int? evasion { get; set; }

        public int? critical { get; set; }
    }
}

