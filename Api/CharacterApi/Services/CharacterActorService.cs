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
using Common.Services.Interfaces;

namespace CharacterApi.Services
{
	public class CharacterActorSerivce : ICharacterService
	{
		private readonly DaprClient _daprClient;
		private readonly ICharacterStoreFactory _characterStoreFactory;
		private const string CHARACTER_FALLBACK_TYPE = "ORDINAL";

		public CharacterActorSerivce(DaprClient daprClient, ICharacterStoreFactory characterStoreFactory)
		{
			_daprClient = daprClient;
			_characterStoreFactory = characterStoreFactory;
		}

		public async Task<CharacterViewModel> CreateChar(string name, string bio)
		{
			var newCharId = System.Guid.NewGuid().ToString("B");
			string archiType = CHARACTER_FALLBACK_TYPE;
			var characterModel = new Character
			{
				Name = name,
				Bio = bio,
				Id = newCharId,
				ActorType = archiType //TODO: character type here
			};
			var store = _characterStoreFactory.CreateCharacterStore(newCharId);
			await store.StoreCharacterAsync(characterModel);
			var proxy = await Helper.GetCharacterActorAsync(newCharId, _characterStoreFactory, archiType);
			await proxy.Speak(new Message
			{
				RecepientId = newCharId,
				MessageText = $"The Hero {characterModel.Name} awaken. The Hero Bio was not easy: {characterModel.Bio}. And {characterModel.Name} came {archiType}"
			});
			//init char info
			return Helper.ConvertCharacter(characterModel);
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
			var proxy = await Helper.GetCharacterActorAsync(characterId, _characterStoreFactory);
			var knownChars = await proxy.GetKnownCharacters();
			var result = new List<CharacterViewModel>();
			await foreach (var character in Helper.ConverViewAsync(knownChars, _characterStoreFactory))
			{
				result.Add(character);
			}

			return result;
		}

		public async Task<IEnumerable<Location>> GetLocations(string characterId)
		{
			var proxy = await Helper.GetCharacterActorAsync(characterId, _characterStoreFactory);
			return await proxy.GetKnownLocations();
		}

		public async Task<IEnumerable<Quest>> GetQuests(string characterId)
		{
			var proxy = await Helper.GetCharacterActorAsync(characterId, _characterStoreFactory);
			return await proxy.GetKnownQuests();
		}

		public Task<CharacterViewModel> GetSelf(string characterId)
		{
			throw new System.NotImplementedException();
		}

		public async Task PerformAction(string characterId, string action, string targetId)
		{
			var proxy = await Helper.GetCharacterActorAsync(characterId, _characterStoreFactory);
			switch (action?.ToLowerInvariant())
			{
				case "move": await proxy.MoveTo(new Location { Id = targetId }); break;
				case "atatck": await proxy.Attack(targetId); break;
				case "say":
					await proxy.Speak(new Message
					{
						RecepientId = targetId,
						MessageText = "Foo text message"
					}); break;
				case "do quest": await proxy.DoQuest(new Quest { Id = targetId }); break;
				case "observe": await proxy.Observe(); break;
				default:
					throw new System.NotSupportedException();
			}
		}

	}
	static class Helper
	{
		static readonly string ActorFallbackType = "ORDINAL";
		public static async Task<ICharacterActor> GetCharacterActorAsync(string characterId, ICharacterStoreFactory storeFactory, string characterType = null)
		{
			var actorId = new ActorId(characterId);
			// TODO: somehow identify character type, please
			//  if no actor type known - we should create new Player
			string actorType = characterType
				?? (await storeFactory.CreateCharacterStore(characterId).GetCharacterAsync()).ActorType
				?? ActorFallbackType;

			return ActorProxy.Create<ICharacterActor>(actorId, actorType);
		}
		public static async IAsyncEnumerable<CharacterViewModel> ConverViewAsync(IEnumerable<string> characterIds, ICharacterStoreFactory storeFactory)
		{
			foreach (var characterId in characterIds)
			{
				var store = storeFactory.CreateCharacterStore(characterId);
				var characterData = await store.GetCharacterAsync();
				yield return ConvertCharacter(characterData);
			}
		}
		public static CharacterViewModel ConvertCharacter(Character character)
		{
			string mappedArhitype = character.ActorType;

			return new CharacterViewModel
			{
				Bio = character.Bio,
				Id = character.Id,
				Name = character.Name,
				ArchiType = mappedArhitype
			};
		}
	}
}