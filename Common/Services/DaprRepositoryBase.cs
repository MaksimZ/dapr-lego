using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Entities;
using Dapr.Client;

namespace Common.Services
{
	abstract class DaprRepositoryBase
	{
		private readonly string _itemId;
		private readonly string _storeItemId;
		private readonly string _storeName;
		private readonly string _itemPrefix;
		private readonly DaprClient _daprClient;
		private readonly ConcurrentDictionary<string, string> _etags = new();
		protected DaprRepositoryBase(string storeName, string itemId, string itemPrefix, DaprClient daprClient)
		{
			_itemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
			_storeName = storeName ?? throw new ArgumentNullException(nameof(storeName));
			_daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
			_itemPrefix = itemPrefix ?? throw new ArgumentNullException(nameof(itemPrefix));

			_storeItemId = $"{_itemPrefix}-{_itemId}";

		}

		protected async Task<T> GetItemAsync<T>(string itemName)
		{
			var (value, etag) = await _daprClient.GetStateAndETagAsync<T>(_storeName, $"{_storeItemId}-{itemName}");
			_etags.AddOrUpdate(itemName, etag, (_,_) => etag);
			return value;
		}

		protected async Task<bool> StoreItemAsync<T>(string itemName, T item) {
			string etag = _etags[itemName];
			return await _daprClient.TrySaveStateAsync(_storeName, _storeItemId, item, etag);
		}
	}
}