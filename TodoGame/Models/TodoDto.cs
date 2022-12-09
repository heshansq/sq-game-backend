using System;
namespace TodoGame.Models
{
	public class TodoDto
	{
		public TodoDto()
		{
		}

		public string title { get; set; }
		public string desc { get; set; }
        public int priority { get; set; }
		public string userid { get; set; }
        public int status { get; set; }
    }
}

