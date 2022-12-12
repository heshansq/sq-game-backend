using System;
namespace TodoGame.Models
{
	public class GameDto
	{
		public GameDto()
		{
		}

        public string? Id { get; set; }
        public int type { get; set; }
        public int status { get; set; }
		public string gamestartuser { get; set; }
		public string gameopponent { get; set; }
    }
}

