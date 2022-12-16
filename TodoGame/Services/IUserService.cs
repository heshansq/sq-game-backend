using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface IUserService
	{
		List<User> GetAllUsers();
        List<User> GetOnlineUsers();
        User GetUser(string id);
		User CreateUser(User user);
		UserLoginDto Authenticate(string email, string password);
		UpdateResult UpdateConnectionId(string userid, string? connectionId);
    }
}

