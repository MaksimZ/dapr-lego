using Common.Entities;

namespace Common.ActorInterfaces.Models
{
	public class Message
	{
		public Character Recepient { get; set; }
		public string MessageText { get; set; }
	}
}