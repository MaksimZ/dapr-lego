using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Entities;
using Dapr.Client;

namespace Common.Services
{
	class DaprQuestRepository : DaprRepositoryBase, Interfaces.IQuestRepository
	{
		private const string PREFIX = "location-info";
		public DaprQuestRepository(string storeName, string questId, DaprClient daprClient)
		: base(storeName, questId, PREFIX, daprClient)
		{
		}
	}
}