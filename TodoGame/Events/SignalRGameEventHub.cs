using System;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TodoGame.Models;

namespace TodoGame.Events
{
    public class SignalRGameEventHub : Hub
	{
		public SignalRGameEventHub()
		{
		}

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task sendGameStartData(SignalRMessage signalRMessage)
        {
            var users = new string[] { signalRMessage.startuserid, signalRMessage.opuserid };
            //await Clients.Users(users).SendAsync(signalRMessage.messageType, signalRMessage);
            var jsonString = JsonConvert.SerializeObject(signalRMessage);
            await Clients.Clients(users).SendAsync(signalRMessage.messageType, jsonString);
        }
    }
}

