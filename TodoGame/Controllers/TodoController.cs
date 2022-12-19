using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TodoGame.Models;
using TodoGame.Services;

namespace TodoGame.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService todoService;
        private readonly IUserService userService;
        private readonly IGameService gameService;

		public TodoController(ITodoService _todoService, IUserService _userService, IGameService _gameService)
		{
            todoService = _todoService;
            userService = _userService;
            gameService = _gameService;
        }

        [HttpGet]
        public IActionResult getAllTodo()
        {
            return Ok(todoService.getAllTodo());
        }

        [HttpGet("{id}")]
        public ActionResult<List<Todo>> getTodoListByUser(string id)
        {
            return todoService.getAllTodoByUser(id);
        }

        [HttpPost]
        public ActionResult<Todo> addTodo(TodoDto todo)
        {
            Todo userTodo = new Todo();
            userTodo.title = todo.title;
            userTodo.desc = todo.desc;
            userTodo.priority = todo.priority;
            userTodo.status = 0;

            User allocatedUser = userService.GetUser(todo.userid);
            userTodo.aluser = allocatedUser;

            return todoService.addTodo(userTodo);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public IActionResult updateTodo([FromBody] TodoDto todo, [FromRoute] string id)
        {
            Todo userTodo = new Todo();
            userTodo.title = todo.title;
            userTodo.desc = todo.desc;
            userTodo.priority = todo.priority;
            userTodo.status = todo.status;

            User allocatedUser = userService.GetUser(todo.userid);

            if (userTodo.status == 2)
            {
                UpdateResult userUpdate = gameService.changeTickets(allocatedUser.Id, allocatedUser.tickets + (userTodo.priority + 1));
            }

            UpdateResult updatedUser = todoService.updateTodo(id, userTodo);
            if (updatedUser.ModifiedCount > 0)
            {
                return Ok(todo);
            }

            return NotFound("Not Found");
        }
    }
}

