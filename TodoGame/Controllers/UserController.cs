using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoGame.Models;
using TodoGame.Services.Impl;

namespace TodoGame.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
	private readonly UserService userService;

    public UserController(UserService _userService)
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
	public ActionResult<User> RegisterUser(User user) {
		return userService.CreateUser(user);
	}

	[AllowAnonymous]
	[Route("authenticate")]
	[HttpPost]
	public ActionResult Login([FromBody] User user)
	{
		var token = userService.Authenticate(user.email, user.password);

		if (token == null)
		{
			return Unauthorized();
		}

		return Ok(new {token, user});
	}

}

