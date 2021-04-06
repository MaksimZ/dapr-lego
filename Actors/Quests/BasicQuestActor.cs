using Dapr.Actors.Runtime;
using System.Threading.Tasks;

namespace Actors.Quests
{
	[Actor(TypeName = "Basic Quest Actor")]
	class BasicQuestActor : Actor, Common.ActorInterfaces.IQuestActor
	{

		public BasicQuestActor(ActorHost host)
			: base(host)
		{ }
		public Task<string> CompleteQuest(string characterId)
		{
			return Task.FromResult($"Quest {Id.GetId()} completed by {characterId}");
		}
	}
}