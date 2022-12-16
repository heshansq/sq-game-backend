using System;
using TodoGame.Models;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TodoGame.Services.Impl
{
	public class UserService: IUserService
    {
		private readonly IMongoCollection<User> _users;
		private readonly string _key;

		public UserService(IDBClient dBClient, IConfiguration configuration)
		{
            _users = dBClient.GetUserCollection();
			_key = configuration.GetSection("JwtKey").ToString();
        }

		public List<User> GetAllUsers() => _users.Find(user => true).ToList();

		public User GetUser(string id) => _users.Find<User>(user => user.Id == id).FirstOrDefault();

		public User CreateUser(User user) {
			_users.InsertOne(user);
			return user;
		}

        public UpdateResult UpdateConnectionId(string userid, string? connectionId)
        {
            var filter = Builders<User>.Filter.Eq("Id", userid);
            var update = Builders<User>.Update.Set("connectionid", connectionId);

            return _users.UpdateOne(filter, update);
        }

        public UserLoginDto Authenticate(string email, string password)
		{
            //var user = _users.Find(x => x.email == email && x.password == password).FirstOrDefault();
            var user = _users.Find(x => x.email == email).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            byte[] bytesSalt = GetBytes(user.storedsalt);
            var isPasswordMatched = VerifyPassword(password, bytesSalt, user.password);


			if (!isPasswordMatched)
			{
				return null;
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.ASCII.GetBytes(_key);

			var claimEmail = new Claim(ClaimTypes.Email, email.ToString());
			var claimIdentifier = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
			var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimIdentifier }, "serverAuth");
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

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

			UserLoginDto userLoginDto = new UserLoginDto();
			userLoginDto.user = user;
			userLoginDto.token = tokenHandler.WriteToken(token);

            return userLoginDto;
		}

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }

        public List<User> GetOnlineUsers() => _users.Find(user => user.connectionid != "" && user.connectionid != null).ToList();
    }
}

