using System;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface ITodoService
	{
		List<Todo> getAllTodoByUser(string userId);
		List<Todo> getAllTodo();
		Todo addTodo(Todo todo);
	}
}

