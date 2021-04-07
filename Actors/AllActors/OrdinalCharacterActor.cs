using Common.ActorInterfaces.Models;
using Common.Entities;
using Common.Services.Interfaces;
using Dapr.Actors.Runtime;
using Dapr.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace Actors.AllActors
{
	[Actor(TypeName = "Ordinal Character")]
	class OrdinalCharacterActor : Actor, Common.ActorInterfaces.ICharacterActor
	{
		private const string KNOWN_LOCATIONS_STATE = "known-locations";
		private const string KNOWN_CHARACTERS_STATE = "known-characters";
		private const string KNOWN_QUESTS_STATE = "known-quests";
		private const string CURRENT_CHAR_STATE = "current-character";
		private const string BASIC_LOCATION_ACTOR = "Basic Location Actor";
		protected readonly DaprClient _daprClient;
		protected readonly IRepositoriesFactory _repositoryFactory;
		protected ICharacterRepository _characterRepo;

		public OrdinalCharacterActor(
			ActorHost host,
			DaprClient daprClient,
			IRepositoriesFactory repositoryFactory)
			: base(host)
		{
			_daprClient = daprClient;
			_repositoryFactory = repositoryFactory;
		}

		protected override Task OnActivateAsync()
		{
			_characterRepo = _repositoryFactory.CreateCharacterRepository(Id.GetId());
			return base.OnActivateAsync();
		}
		protected override Task OnDeactivateAsync()
		{
			_characterRepo = null;
			return base.OnDeactivateAsync();
		}
		public virtual async Task Attack(string characterId)
		{
			Logger.LogInformation("{thisId} attack {targetId}", Id.GetId(), characterId);

			var charImLookingFor = await _repositoryFactory.CreateCharacterRepository(characterId).GetCharacterAsync();
			if (charImLookingFor != null)
			{
				await SendSelfMessage($"I cant't fight. I'm just an ordinary person. Moreover, {charImLookingFor.Name} looks so powerful...");
			}
			else
			{
				await SendSelfMessage($"So strange to fight with fantasy character {characterId}");
			}
		}

		public async Task DoQuest(Quest quest)
		{
			var knownQuests = await GetKnownQuestIdsState();
			if (knownQuests.Contains(quest.Id))
			{
				await SendSelfMessage($"I'm not an adventurer to do that quest \"{quest.Id}\"");
			}
			else
			{
				await SendSelfMessage($"I've never heard about the quest \"{quest.Id}\"");
			}
		}

		public Task<IEnumerable<string>> GetKnownCharacters()
		{
			return GetKnownCharacterIdsState();
		}

		public async Task<IEnumerable<Location>> GetKnownLocations()
		{
			var knownLocations = await GetKnownLocationIdsState();
			return knownLocations
				.Select(l => new Location
				{
					Id = l,
					Description = "just request it using API"
				});
		}

		public async Task<IEnumerable<Quest>> GetKnownQuests()
		{
			var knownQuests = await GetKnownQuestIdsState();
			return knownQuests
				.Select(q => new Quest
				{
					Id = q,
					LocationId = null,
					Description = "just request it using API"
				});
		}

		public async Task MoveTo(Location location)
		{
			Logger.LogInformation("Moving {char} to location {location}", Id.GetId(), location?.Id);
			// Am I know this location?
			var knownLocations = await GetKnownLocationIdsState();
			if (knownLocations.Contains(location.Id))
			{
				var myState = await GetState();

				//We're leaving location
				Logger.LogInformation("Moving from {location}", myState.LocationId);
				var currentLocationActorId = new Dapr.Actors.ActorId(myState.LocationId);
				var currentLocationActor = this.ProxyFactory.CreateActorProxy<Common.ActorInterfaces.ILocationActor>(currentLocationActorId, BASIC_LOCATION_ACTOR);
				await currentLocationActor.CharacterLeaveLocation(this.Id.GetId());

				// And entering the new one
				Logger.LogInformation("Moving to {location}", location.Id);
				var targetLocationActorId = new Dapr.Actors.ActorId(location.Id);
				var targetLocationActor = this.ProxyFactory.CreateActorProxy<Common.ActorInterfaces.ILocationActor>(targetLocationActorId, BASIC_LOCATION_ACTOR);
				await targetLocationActor.CharacterEnterLocation(this.Id.GetId());

				myState.LocationId = location.Id;
				await this.StateManager.SetStateAsync(CURRENT_CHAR_STATE, myState);


				await SendSelfMessage("I just moved somewhere");
			}
			else
			{
				Logger.LogInformation("Char don't know location {locationid}", location.Id);
				await SendSelfMessage($"I don't know how to get to {location.Id}");
			}
		}

		public async Task Observe()
		{
			var myState = await GetState();
			var actorId = new Dapr.Actors.ActorId(myState.LocationId);
			var currentLocationActor = this.ProxyFactory.CreateActorProxy<Common.ActorInterfaces.ILocationActor>(actorId, BASIC_LOCATION_ACTOR);

			var connectedLocations = await currentLocationActor.ObserveConnectedLocations();
			var knownLocations = await GetKnownLocationIdsState();
			var newLocations = knownLocations
				.Concat(connectedLocations)
				.Distinct()
				.OrderBy(l => l)
				.ToArray();
			await this.StateManager.SetStateAsync(KNOWN_LOCATIONS_STATE, newLocations);

			//TODO: send observation notification
			await SendSelfMessage($"I've found new locations while observing current: {string.Join(", ", connectedLocations)}");

			try
			{

				// TODO: get quests from location
				var locationRepo = _repositoryFactory.CreateLocationRepository(myState.LocationId);

				var questsToMerge = await Task.WhenAll(
					GetKnownQuestIdsState(),
					locationRepo.GetQuestsInLocationAsync()
				);
				var newQuests = questsToMerge
					.SelectMany(q => q)
					.Distinct()
					.OrderBy(q => q)
					.ToArray();
				await Task.WhenAll(
					this.StateManager.SetStateAsync(KNOWN_QUESTS_STATE, newQuests),
					SendSelfMessage($"I've found new quests while observing current: {string.Join(", ", questsToMerge[1])}")
				);
			}
			catch (Exception e)
			{
				Logger.LogError(e, "{actortype}::{actorid} failed to update quests from location", this.GetType(), Id.GetId());
			}
			await this.StateManager.SaveStateAsync();
		}

		public async Task Speak(Message message)
		{
			var knownChars = await GetKnownCharacterIdsState();
			if (knownChars.Contains(message.RecepientId))
			{
				await SendMessage(message.MessageText, message.RecepientId);
			}
			else
			{
				await SendSelfMessage($"Should I worry when I'm trying to speak with those who I don't know? \"{message.MessageText}\" - heh");
			}
		}

		public virtual async Task<string> GetHit(string byCharacterId)
		{
			await SendMessage("Please don't hurt me!", byCharacterId);
			await SendSelfMessage($"{byCharacterId} want's to kill me!");
			return "cry, baby, cry";
		}

		protected async Task<OrdinalCharacterState> GetState()
		{
			var currentState = await this.StateManager.TryGetStateAsync<OrdinalCharacterState>(CURRENT_CHAR_STATE);
			if (currentState.HasValue)
			{
				return currentState.Value;
			}
			else
			{
				var initialState = new OrdinalCharacterState
				{
					CurrentAction = "Idle",
					CurrentQuestId = string.Empty,
					LocationId = "unknown"// need to be some initial location
				};
				await this.StateManager.AddStateAsync(CURRENT_CHAR_STATE, initialState);
				await SetCurrentLocation();
				return initialState;
			}
		}
		protected async Task<IEnumerable<string>> GetKnownCharacterIdsState()
		{
			return await this.StateManager.GetOrAddStateAsync(KNOWN_CHARACTERS_STATE, Array.Empty<string>());
		}
		protected async Task<IEnumerable<string>> GetKnownQuestIdsState()
		{
			return await this.StateManager.GetOrAddStateAsync(KNOWN_QUESTS_STATE, Array.Empty<string>());
		}
		protected async Task<IEnumerable<string>> GetKnownLocationIdsState()
		{
			return await this.StateManager.GetOrAddStateAsync(KNOWN_LOCATIONS_STATE, new string[] {
				//at least initial location
				(await GetState()).LocationId
			});
		}
		protected async Task SendSelfMessage(string message)
		{
			Logger.LogInformation("Sending self message from {senderId} with text {message}", this.Id.GetId(), message);
			await _daprClient.PublishEventAsync("messages-channel", "self", new Message
			{
				MessageText = message,
				RecepientId = this.Id.GetId()
			});
		}
		protected async Task SendMessage(string message, string characterId)
		{
			Logger.LogInformation("Sending personal message from {senderId} to {recepientId} with text {message}", this.Id.GetId(), characterId, message);
			await _daprClient.PublishEventAsync("messages-channel", "personal", new Message
			{
				MessageText = message,
				RecepientId = characterId
			});
		}
		protected async Task SetCurrentLocation()
		{
			var storeCharacter = await _characterRepo.GetCharacterAsync();
			storeCharacter.LocationId = (await this.StateManager.GetStateAsync<OrdinalCharacterState>(CURRENT_CHAR_STATE)).LocationId;
			await _characterRepo.StoreCharacterAsync(storeCharacter);
		}
	}
}