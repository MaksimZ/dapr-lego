using System;
using System.Threading.Tasks;
using Common.Entities;
using Dapr.Client;

namespace Common.Services
{
	class DaprCharacterStore : Interfaces.ICharacterStore
	{
		private readonly string _characterId;
		private readonly string _storeItemId;
		private readonly string _storeName;
		private readonly DaprClient _daprClient;
		private string _etag = string.Empty;
		public DaprCharacterStore(string storeName, string charId, DaprClient daprClient)
		{
			_characterId = charId ?? throw new ArgumentNullException(nameof(charId));
			_storeName = storeName ?? throw new ArgumentNullException(nameof(storeName));
			_daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
			_storeItemId = $"character-info-{_characterId}";

		}
		public async Task<Character> GetCharacterAsync()
		{
			var (character, etag) = await _daprClient.GetStateAndETagAsync<Character>(_storeName, _storeItemId);
			_etag = etag;
			return character;
		}

		public async Task StoreCharacterAsync(Character character)
		{
			const int MAX_TRIES = 3;
            int tries = 0;
			while (tries < MAX_TRIES && !await _daprClient.TrySaveStateAsync(_storeName, _storeItemId, character, _etag))
			{
				_etag = (await _daprClient.GetStateAndETagAsync<Character>(_storeName, _storeItemId)).etag;
                ++tries;
			}
            if (tries >= MAX_TRIES) {
                //store error
                //Log and throw
                
            }
		}

	}
}