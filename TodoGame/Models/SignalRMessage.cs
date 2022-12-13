using System;
namespace TodoGame.Models
{
	public class SignalRMessage
	{
		public SignalRMessage()
		{
		}

		public string startuserid { get; set; }
		public string opuserid { get; set; }
		public string message { get; set; }
		public string messageType { get; set; }
	}
}

