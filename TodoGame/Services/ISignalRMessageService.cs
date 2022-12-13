using System;
using TodoGame.Models;

namespace TodoGame.Services
{
	public interface ISignalRMessageService
    {
        Task sendGameStatusNotificationAsync(SignalRMessage signalRMessage);
    }
}

