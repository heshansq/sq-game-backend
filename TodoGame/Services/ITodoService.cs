using System;
using MongoDB.Driver;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface ITodoService
	{
		List<Todo> getAllTodoByUser(string userId);
		List<Todo> getAllTodo();
		Todo addTodo(Todo todo);
        UpdateResult updateTodo(string id, Todo todo);
	}
}

