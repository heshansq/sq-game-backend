using System;
using TodoGame.Models;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TodoGame.Services.Impl
{
	public class UserService
	{
		private readonly IMongoCollection<User> _users;
		private readonly string _key;

		public UserService(IDBClient dBClient, IConfiguration configuration)
		{
            _users = dBClient.GetUserCollection();
			_key = configuration.GetSection("JwtKey").ToString();
        }

		public List<User> GetAllUsers() => _users.Find(user => true).ToList();

		public User GetUser(string id) => _users.Find<User>(user => user.id == id).FirstOrDefault();

		public User CreateUser(User user) {
			_users.InsertOne(user);
			return user;
		}

		public string Authenticate(string email, string password)
		{
			var user = _users.Find(x => x.email == email && x.password == password).FirstOrDefault();

			if (user == null)
			{
				return null;
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.ASCII.GetBytes(_key);

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Email, email)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
    }
}

