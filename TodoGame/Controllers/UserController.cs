using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TodoGame.Models;
using TodoGame.Services;
using TodoGame.Services.Impl;

namespace TodoGame.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
	private readonly IUserService userService;

    public UserController(IUserService _userService)
	{
		userService = _userService;
	}

	[HttpGet]
	public ActionResult<List<User>> GetUsers()
	{
		return userService.GetAllUsers();
	}

	[HttpGet("{id}")]
	public ActionResult<User> GetUser(string id) {
		return userService.GetUser(id);
	}

	[HttpPost]
    [AllowAnonymous]
    public ActionResult<User> RegisterUser(User user) {

        var hashedPassword = EncryptPassword(user.password);
        User saveUser = new User();

        var str = GetString(hashedPassword.salt);

        saveUser.password = hashedPassword.hash;
        saveUser.storedsalt = str;
        saveUser.email = user.email;
        saveUser.tickets = 10;
        saveUser.connectionid = "";
        //saveUser.tickets = 0;

        return userService.CreateUser(saveUser);
	}

	[AllowAnonymous]
	[Route("authenticate")]
	[HttpPost]
	public ActionResult Login([FromBody] User user)
	{
        UserLoginDto userLoginDto = userService.Authenticate(user.email, user.password);

        var token = userLoginDto.token;

		if (token == null)
		{
			return Unauthorized();
		}

		return Ok(userLoginDto);
	}

    [AllowAnonymous]
    [Route("connectionid")]
    [HttpPost]
    public ActionResult UpdateConnectionId([FromBody] ConnectionDto connectionDto)
    {
        UpdateResult userUpdate = userService.UpdateConnectionId(connectionDto.userId, connectionDto.connectionId);
        
        return Ok(userUpdate);
    }

    public static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public static HashSalt EncryptPassword(string password)
    {
        byte[] salt = new byte[128 / 8]; 
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));
        return new HashSalt { hash = encryptedPassw, salt = salt };
    }

}

