using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Entities;
using Dapr.Client;

namespace Common.Services
{
	class DaprLocationRepository : DaprRepositoryBase, Interfaces.ILocationRepository
	{
		private const string PREFIX = "location-info";
		private const string LOC_ITEM_NAME = "location";
		private const string CHARS_ITEM_NAME = "characters";
		private const string QUESTS_ITEM_NAME = "quests";
		public DaprLocationRepository(string storeName, string locationId, DaprClient daprClient)
		: base(storeName, locationId, PREFIX, daprClient)
		{
		}
		public Task<IEnumerable<string>> GetCharactersInLocationAsync() => GetItemAsync<IEnumerable<string>>(CHARS_ITEM_NAME);

		public Task<Location> GetLocationAsync() => GetItemAsync<Location>(LOC_ITEM_NAME);

		public Task<IEnumerable<string>> GetQuestsInLocationAsync() => GetItemAsync<IEnumerable<string>>(QUESTS_ITEM_NAME);

		public Task<bool> SetLocationAsync(Location location) => StoreItemAsync(LOC_ITEM_NAME, location);

		public Task<bool> SetQuestsInLocationAsync(IEnumerable<string> quests) => StoreItemAsync(QUESTS_ITEM_NAME, quests);

		public Task<bool> StoreCharactersInLocationAsync(IEnumerable<string> charIds) => StoreItemAsync(CHARS_ITEM_NAME, charIds);
	}
}