using System;
using System.Security.Claims;

namespace TodoGame.Models
{
	public class UserLoginDto
	{
		public UserLoginDto()
		{
		}

		public User user { get; set; }
		public string token { get; set; }
    }
}

