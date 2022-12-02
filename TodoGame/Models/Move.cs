using System;
namespace TodoGame.Models
{
	public class Move
	{
		public string name { get; set; }

		public string type { get; set; }

		public string category { get; set; }

		public int power { get; set; }

		public int accuracy { get; set; }

		public int pp { get; set; }

		public bool contact { get; set; }

		public object statChanges { get; set; }

    }
}

