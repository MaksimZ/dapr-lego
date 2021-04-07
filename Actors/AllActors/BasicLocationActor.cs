using System;
using Dapr.Actors.Runtime;
using Dapr.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Common.ActorInterfaces.Models;
using Common.Services.Interfaces;

namespace Actors.AllActors
{

	[Actor(TypeName = "Basic Location Actor")]
	class BasicLocationActor : Actor, Common.ActorInterfaces.ILocationActor
	{
		private readonly DaprClient _daprClient;
		private readonly IRepositoriesFactory _repositoriesFactory;
		private ILocationRepository _locationRepo;
		public BasicLocationActor(ActorHost host, DaprClient daprClient, IRepositoriesFactory repositoriesFactory)
			: base(host)
		{
			_daprClient = daprClient;
			_repositoriesFactory = repositoriesFactory;
		}

		protected override Task OnActivateAsync()
		{
			_locationRepo = _repositoriesFactory.CreateLocationRepository(Id.GetId());
			return base.OnActivateAsync();
		}

		protected override Task OnDeactivateAsync()
		{
			_locationRepo = null;
			return base.OnDeactivateAsync();
		}

		public async Task CharacterEnterLocation(string characterId)
		{
			int tries = 0;
			const int MAX_TRIES = 3;
			bool saved = false;
			try
			{
				do
				{
					tries++;
					var chars = await _locationRepo.GetCharactersInLocationAsync();
					var newChars = chars
						.Append(characterId)
						.OrderBy(s => s)
						.ToArray();
					saved = await _locationRepo.StoreCharactersInLocationAsync(newChars);
				}
				while (!saved && tries < MAX_TRIES);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to store character {char} in locaiton {loc}", characterId, Id.GetId());
			}
			if (tries >= MAX_TRIES)
			{
				//we have saving error
				Logger.LogError("Failed to store location {loc} entering characters due tries exceeded", Id.GetId());
			}
			else
			{   //everything is ok
				await SendLocationEvent($"Entering locaiton {this.Id.GetId()}", characterId);

			}
		}

		public async Task CharacterLeaveLocation(string characterId)
		{
			int tries = 0;
			const int MAX_TRIES = 3;
			bool saved = false;
			try
			{
				do
				{
					tries++;
					var chars = await _locationRepo.GetCharactersInLocationAsync();
					var newChars = chars
						.Where(c => !characterId.Equals(c))
						.OrderBy(s => s)
						.ToArray();
					saved = await _locationRepo.StoreCharactersInLocationAsync(newChars);
				}
				while (!saved && tries < MAX_TRIES);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to remove character {char} from locaiton {loc}", characterId, Id.GetId());
			}
			if (tries >= MAX_TRIES)
			{
				//we have saving error
				Logger.LogError("Failed to store location {loc} leaving characters due tries exceeded", Id.GetId());
			}
			else
			{   //everything is ok
				await SendLocationEvent($"Leaving location {this.Id.GetId()}", characterId);
			}
		}

		public async Task<IEnumerable<string>> ObserveConnectedLocations()
		{
			var location = await _locationRepo.GetLocationAsync();
			if (location == null) {
				Logger.LogWarning("Location {loc} has no information", Id.GetId());
			}
			return location?.ConnectedLocations ?? Array.Empty<string>();
		}

		public Task<string> RequestQuest(string characterId, string questId)
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