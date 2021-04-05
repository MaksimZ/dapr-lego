using Dapr.Actors.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Actors.Locations
{

	[Actor(TypeName = "Basic Location Actor")]
	class BasicLocationActor : Actor, Common.ActorInterfaces.ILocationActor
	{
		public BasicLocationActor(ActorHost host)
			: base(host)
		{ }
		public Task CharacterEnterLocation(string characterId)
		{
			throw new System.NotImplementedException();
		}

		public Task CharacterLeaveLocation(string characterId)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<string>> ObserveConnectedLocations()
		{
			throw new System.NotImplementedException();
		}

		public Task<string> TakeQuest(string characterId, string questId)
		{
			throw new System.NotImplementedException();
		}
	}
}