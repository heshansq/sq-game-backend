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

		public User GetUser(string id) => _users.Find<User>(user => user.Id.ToString() == id).FirstOrDefault();

		public User CreateUser(User user) {
			_users.InsertOne(user);
			return user;
		}

		public string Authenticate(string email, string password)
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
    }
}

