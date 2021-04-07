using Common.Services.Interfaces;
using Dapr.Client;
namespace Common.Services
{
	class DaprRepositoriesFactory : IRepositoriesFactory
	{
		private readonly DaprClient _daprClient;
		private const string CHARACTER_STORAGE = "characters-store";
		private const string LOCATION_STORAGE = "location-store";
		private const string QUEST_STORAGE = "quest-store";
		public DaprRepositoriesFactory(DaprClient daprClient)
		{
			_daprClient = daprClient;
		}

		public ICharacterRepository CreateCharacterRepository(string characterId) => new DaprCharacterRepository(CHARACTER_STORAGE, characterId, _daprClient);

		public ILocationRepository CreateLocationRepository(string locationId) => new DaprLocationRepository(LOCATION_STORAGE, locationId, _daprClient);

		public IQuestRepository CreateQuestRepository(string questId) => new DaprQuestRepository(QUEST_STORAGE, questId, _daprClient);
	}
}