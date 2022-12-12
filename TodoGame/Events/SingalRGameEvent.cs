using System;
using Microsoft.AspNetCore.SignalR;

namespace TodoGame.Events
{
	public class SingalRGameEvent : Hub
	{
		public SingalRGameEvent()
		{
		}

        public async Task sendGameStartData(string gameStartMessage)
        {
            await Clients.All.SendAsync("newMessage", "anonymous", gameStartMessage);
        }
    }
}

