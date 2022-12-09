using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services.Impl
{
	public class TodoService : ITodoService
    {
        public readonly IMongoCollection<Todo> _todoList;
        private readonly IUserService userService;

        public TodoService(IDBClient dBClient, IUserService _userService)
		{
            _todoList = dBClient.GetTodoCollection();
            userService = _userService;
		}

        public Todo addTodo(Todo todo)
        {
            _todoList.InsertOne(todo);
            return todo;
        }

        public List<Todo> getAllTodo() => _todoList.Find(TodoLt => true).ToList();

        public List<Todo> getAllTodoByUser(string userId)
        {
            return _todoList.Find(todo => todo.aluser.Id == userId).ToList();
        }
    }
}

