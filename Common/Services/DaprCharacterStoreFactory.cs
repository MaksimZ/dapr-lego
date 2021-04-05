using Common.Services.Interfaces;
using Dapr.Client;
namespace Common.Services
{
    class DaprCharacterStoreFactory: Interfaces.ICharacterStoreFactory
    {
        private readonly DaprClient _daprClient;
        private const string CHARACTER_STORAGE = "characters-store";
        public DaprCharacterStoreFactory(DaprClient daprClient) {
            _daprClient = daprClient;
        }

		public ICharacterStore CreateCharacterStore(string characterId)
		{
			return new DaprCharacterStore(CHARACTER_STORAGE, characterId, _daprClient);
		}
	}
}