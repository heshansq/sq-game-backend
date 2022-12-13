using System;
using Microsoft.AspNetCore.SignalR;
using TodoGame.Models;

namespace TodoGame.Events
{
	public class SingalRGameEvent : Hub
	{
		public SingalRGameEvent()
		{
		}

        public async Task sendGameStartData(SignalRMessage signalRMessage)
        {
            var users = new string[] { signalRMessage.startuserid, signalRMessage.opuserid };
            await Clients.Users(users).SendAsync(signalRMessage.messageType, signalRMessage);
        }
    }
}

