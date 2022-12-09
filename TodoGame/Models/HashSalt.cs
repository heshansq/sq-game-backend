using System;
namespace TodoGame.Models
{
	public class HashSalt
	{
		public HashSalt()
		{
		}

		public string hash { get; set; }
		public byte[] salt { get; set; }
	}
}

