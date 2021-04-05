using Dapr.Actors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.ActorInterfaces
{
	public interface ILocationActor : IActor
	{
		Task CharacterEnterLocation(string characterId);
		Task CharacterLeaveLocation(string characterId);
		Task<string> TakeQuest(string characterId, string questId);
        Task<IEnumerable<string>> ObserveConnectedLocations();
	}
}