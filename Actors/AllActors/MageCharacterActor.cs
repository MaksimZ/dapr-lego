using Common.ActorInterfaces.Models;
using Common.Entities;
using Common.Services.Interfaces;
using Dapr.Actors.Runtime;
using Dapr.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using Common.ActorInterfaces;

namespace Actors.AllActors
{
	[Actor(TypeName = "Mage Character")]
	class MageCharacterActor : OrdinalCharacterActor, ICharacterActor
	{

		public MageCharacterActor(
			ActorHost host,
			DaprClient daprClient,
			IRepositoriesFactory repositoryFactory)
			: base(host, daprClient, repositoryFactory)
		{
		}

		public override async Task Attack(string characterId)
		{
			Logger.LogInformation("{thisId} attack {targetId}", Id.GetId(), characterId);

			var charImLookingFor = await _repositoryFactory.CreateCharacterRepository(characterId).GetCharacterAsync();
			if (charImLookingFor != null)
			{
				var targetActor = ProxyFactory.CreateActorProxy<ICharacterActor>(new Dapr.Actors.ActorId(characterId), charImLookingFor.ActorType);
				await Task.WhenAll(
					SendSelfMessage($"I think fireball will be enough for {charImLookingFor.Name}"),
					SendMessage($"{charImLookingFor.Name}, you'll feel my Power!", characterId)
				);
				var response = await targetActor.GetHit(Id.GetId());
				await SendSelfMessage($"Have {charImLookingFor.Name} just said {response} ?");
			}
			else
			{
				await SendSelfMessage($"So strange to fight with fantasy character {characterId}");
			}
		}

		public override async Task<string> GetHit(string byCharacterId)
		{
			await SendMessage("Please don't hurt me!", byCharacterId);
			await SendSelfMessage($"{byCharacterId} want's to kill me!");
			return "cry, baby, cry";
		}
	}
}