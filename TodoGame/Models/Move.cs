using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGame.Models
{
    [BsonIgnoreExtraElements]
    public class Move
	{
		public string? name { get; set; }

		public string? type { get; set; }

		public string? category { get; set; }

		public int? power { get; set; }

		public bool? causeKo { get; set; }

		public bool? levelDamage { get; set; }

		public int? retaliate { get; set; }

		public int? accuracy { get; set; }

		public int? accuracyLevelDifference { get; set; }

        public int? pp { get; set; }

		public bool? contact { get; set; }

		public int? priority { get; set; }

		//public List<int>? attacks { get; set; }

		public int? charge { get; set; }

		public int? critBonus { get; set; }

		//public string? drain { get; set; }

        public StatChanges? statChanges { get; set; }

		public StatusChanges? statusChanges { get; set; }

		public int? invulnerable { get; set; }

		public int? recoil { get; set; }

		public bool? swap { get; set; }

    }
}

