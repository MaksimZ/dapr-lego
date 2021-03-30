using System;

namespace ActorInterfaces.Models
{
	public class Message
	{
		public Entities.Character Recepient { get; set; }
		public string MessageText { get; set; }
	}
}