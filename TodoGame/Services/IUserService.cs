using System;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface IUserService
	{
		List<User> GetAllUsers();
		User GetUser(string id);
		User CreateUser(User user);
		string Authenticate(string email, string password);
    }
}

