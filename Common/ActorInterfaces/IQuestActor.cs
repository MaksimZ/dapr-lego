using Dapr.Actors;
using System.Threading.Tasks;

namespace Common.ActorInterfaces
{
	public interface IQuestActor : IActor
	{
        Task<string> CompleteQuest(string characterId);
	}
}