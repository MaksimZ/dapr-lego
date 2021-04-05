using Common.ActorInterfaces.Models;
using Common.Entities;
using Dapr.Actors.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Actors.Characters
{
	[Actor(TypeName = "Ordinal Character")]
	class OrdinalActor : Actor, Common.ActorInterfaces.ICharacterActor
	{
		public OrdinalActor(ActorHost host)
			: base(host)
		{

		}

		protected override Task OnActivateAsync()
		{

			return base.OnActivateAsync();
		}
		protected override Task OnDeactivateAsync()
		{
			return base.OnDeactivateAsync();
		}
		public Task Attack(string characterId)
		{
			throw new System.NotImplementedException();
		}

		public Task DoQuest(Quest quest)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<string>> GetKnownCharacters()
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<Location>> GetKnownLocations()
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<Quest>> GetKnownQuests()
		{
			throw new System.NotImplementedException();
		}

		public Task MoveTo(Location location)
		{
			throw new System.NotImplementedException();
		}

		public Task Observe()
		{
			throw new System.NotImplementedException();
		}

		public Task Speak(Message message)
		{
			System.Console.WriteLine($"New Message for {message.RecepientId}\n{message.MessageText}");
			return Task.CompletedTask;
		}
	}
}