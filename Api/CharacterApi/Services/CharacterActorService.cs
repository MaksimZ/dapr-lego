using CharacterApi.Models;
using Common.Entities;
using Common.ActorInterfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dapr.Client;
using Dapr.Actors;
using Dapr.Actors.Client;
using Common.ActorInterfaces;


namespace CharacterApi.Services
{
	public class CharacterActorSerivce : ICharacterService
	{
		private readonly DaprClient _daprClient;
		private const string CHARACTER_STORAGE = "characters-store";

		public CharacterActorSerivce(DaprClient daprClient)
		{
			_daprClient = daprClient;
		}

		public async Task<CharacterViewModel> CreateChar(string name, string bio)
		{
			var newCharId = System.Guid.NewGuid().ToString("B");
			object characterModel = null;
			var stateEntry = await _daprClient.GetStateEntryAsync<object>(CHARACTER_STORAGE, $"charinfo-{newCharId}");
			stateEntry.Value = characterModel;
			await stateEntry.TrySaveAsync();
			//init char info
			return new CharacterViewModel
			{
				Bio = bio,
				Id = newCharId,
				Name = name
			};
		}

		public Task<IEnumerable<ActionViewModel>> GetActions(string characterId)
		{
			return Task.FromResult(new[] {
				new ActionViewModel {
					Name = "Move",
					Status = "None",
					Description= "Move character to Location",
					Target = string.Empty
				},
				new ActionViewModel {
					Name = "Attack",
					Status = "None",
					Description= "Attack character",
					Target = string.Empty
				},
				new ActionViewModel {
					Name = "Say",
					Status = "None",
					Description= "Send Message",
					Target = string.Empty
				},
				new ActionViewModel {
					Name = "Do Quest",
					Status = "None",
					Description= "Enforces to do Quest",
					Target = string.Empty
				},
				new ActionViewModel {
					Name = "Observe",
					Status = "None",
					Description= "Character observe current location",
					Target = string.Empty
				}
			}.AsEnumerable());
		}

		public async Task<IEnumerable<CharacterViewModel>> GetCharacters(string characterId)
		{
			var proxy = Helper.GetCharacterActor(characterId);
			var knownChars = await proxy.GetKnownCharacters();
			var result = new List<CharacterViewModel>();
			await foreach (var character in Helper.ConverViewAsync(knownChars))
			{
				result.Add(character);
			}
			return result;
		}

		public Task<IEnumerable<Location>> GetLocations(string characterId)
		{
			var proxy = Helper.GetCharacterActor(characterId);
			return proxy.GetKnownLocations();
		}

		public Task<IEnumerable<Quest>> GetQuests(string characterId)
		{
			var proxy = Helper.GetCharacterActor(characterId);
			return proxy.GetKnownQuests();
		}

		public Task<CharacterViewModel> GetSelf(string characterId)
		{
			throw new System.NotImplementedException();
		}

		public Task PerformAction(string characterId, string action, string targetId)
		{
			var proxy = Helper.GetCharacterActor(characterId);
			switch (action?.ToLowerInvariant())
			{
				case "move": return proxy.MoveTo(new Location { Id = targetId });
				case "atatck": return proxy.Attack(new Character { Id = targetId });
				case "say":
					return proxy.Speak(new Message
					{
						Recepient = new Character { Id = targetId },
						MessageText = "Foo text message"
					});
				case "do quest": return proxy.DoQuest(new Quest { Id = targetId });
				case "observe": return proxy.Observe();
				default:
					throw new System.NotSupportedException();
			}
		}

	}
	static class Helper
	{
		public static ICharacterActor GetCharacterActor(string characterId)
		{
			var actorId = new ActorId(characterId);
			// TODO: somehow identify character type, please
			//  if no actor type known - we should create new Player

			string characterType = string.Empty;
			return ActorProxy.Create<ICharacterActor>(actorId, characterType);
		}
		public static async IAsyncEnumerable<CharacterViewModel> ConverViewAsync(IEnumerable<Character> characters)
		{
			foreach (var character in characters)
			{
				// var actorId = new ActorId(character.Id);
				// var proxy = ActorProxy.Create<ICharacterActor>(actorId, "SOMEACTORTYPE, IT SHOULD BE SOMETHING LIKE PLAYER");
				// var knownChars = await proxy.();
				// TODO Get some data from storage ?
				yield return new CharacterViewModel();
			}
		}
	}
}