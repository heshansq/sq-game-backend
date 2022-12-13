using System;
using Microsoft.AspNetCore.SignalR;
using TodoGame.Events;
using TodoGame.Models;

namespace TodoGame.Services.Impl
{
	public class SignalRMessageService : ISignalRMessageService
    {
		private readonly IHubContext<SingalRGameEvent> _hub;

        public SignalRMessageService(IHubContext<SingalRGameEvent> hub)
		{
			_hub = hub;
		}

        public Task sendGameStatusNotificationAsync(SignalRMessage signalRMessage)
        {
            var users = new string[] { signalRMessage.startuserid, signalRMessage.opuserid };
            return _hub.Clients.Users(users).SendAsync(signalRMessage.messageType, signalRMessage);
        }
    }
}

