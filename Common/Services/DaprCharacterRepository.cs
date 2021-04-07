using System;
using System.Threading.Tasks;
using Common.Entities;
using Dapr.Client;

namespace Common.Services
{
	class DaprCharacterRepository : DaprRepositoryBase, Interfaces.ICharacterRepository
	{
		private const string PREFIX = "character-info";
		private const string CHAR_ITEM_NAME = "character";
		public DaprCharacterRepository(string storeName, string charId, DaprClient daprClient)
		: base(storeName, charId, PREFIX, daprClient)
		{
		}
		public Task<Character> GetCharacterAsync() => GetItemAsync<Character>(CHAR_ITEM_NAME);

		public Task<bool> StoreCharacterAsync(Character character) => StoreItemAsync(CHAR_ITEM_NAME, character);

	}
}