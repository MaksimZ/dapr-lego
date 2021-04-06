using Dapr.Actors.Runtime;
using Dapr.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Common.ActorInterfaces.Models;

namespace Actors.AllActors
{

	[Actor(TypeName = "Basic Location Actor")]
	class BasicLocationActor : Actor, Common.ActorInterfaces.ILocationActor
	{
		private readonly DaprClient _daprClient;
		public BasicLocationActor(ActorHost host, DaprClient daprClient)
			: base(host)
		{
			_daprClient = daprClient;
		}
		public Task CharacterEnterLocation(string characterId)
		{
			return SendLocationEvent($"Entering locaiton {this.Id.GetId()}", characterId);
		}

		public Task CharacterLeaveLocation(string characterId)
		{
			return SendLocationEvent($"Leaving location {this.Id.GetId()}", characterId);
		}

		public Task<IEnumerable<string>> ObserveConnectedLocations()
		{
			return Task.FromResult(new string[] {
				"City:Foo|Street:West",
				"City:Foo|Street:South",
				"City:Foo|Street:East",
				"City:Foo|Street:North",
			}.AsEnumerable());
		}

		public Task<string> TakeQuest(string characterId, string questId)
		{
			Logger.LogInformation("Quest {questId} requested by {charId} in location {locId}", questId, characterId, this.Id.GetId());
			return Task.FromResult(string.Empty);
		}

		private async Task SendLocationEvent(string message, string characterId)
		{
			Logger.LogInformation("Location from {senderId} to {recepientId} with message {message}", this.Id.GetId(), characterId, message);
			await _daprClient.PublishEventAsync("notifications", "location", new Message
			{
				MessageText = message,
				RecepientId = characterId
			});
		}
	}
}