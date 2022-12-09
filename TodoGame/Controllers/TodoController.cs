using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoGame.Models;
using TodoGame.Services;

namespace TodoGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService todoService;
        private readonly IUserService userService;

		public TodoController(ITodoService _todoService, IUserService _userService)
		{
            todoService = _todoService;
            userService = _userService;
        }

        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult<Todo> addTodo(TodoDto todo)
        {
            Todo userTodo = new Todo();
            userTodo.title = todo.title;
            userTodo.desc = todo.desc;
            userTodo.priority = todo.priority;

            User allocatedUser = userService.GetUser(todo.userid);
            userTodo.aluser = allocatedUser;

            return todoService.addTodo(userTodo);
        } 
    }
}

