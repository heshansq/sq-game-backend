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
		public string gameId { get; set; }
		public int messagetype { get; set; }
		public string startuserpublickey { get; set; }
        public string opuserpublickey { get; set; }
    }
}

