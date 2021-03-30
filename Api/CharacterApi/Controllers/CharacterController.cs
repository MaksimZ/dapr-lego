using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharacterApi.Models;
using Microsoft.Extensions.Logging;

namespace CharacterApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	public class CharacterController : ControllerBase
	{
		private readonly Services.ICharacterService _characterService;

		public CharacterController(Services.ICharacterService characterService)
		{
			_characterService = characterService;
		}

		[HttpGet("{id}")]
		public async Task<CharacterViewModel> GetCharacter(string id)
		{
			return await _characterService.GetSelf(id);
		}

		[HttpPost()]
		[Consumes("application/json")]
		public async Task<CharacterViewModel> CreateCharacter([FromBody] CharacterViewModel viewModel)
		{
			return await _characterService.CreateChar(viewModel.Name, viewModel.Bio);
		}

		[HttpGet("{id}/characters")]
		public async Task<IEnumerable<CharacterViewModel>> GetCharacters(string id)
		{
			return await _characterService.GetCharacters(id);
		}

		[HttpGet("{id}/quests")]
		[HttpGet("{id}/quests/{questId?}")]
		public async Task<IEnumerable<QuestViewModel>> GetKnownQuests(string id, string questId)
		{
			var quests = await _characterService.GetQuests(id);
			if (questId != null)
			{
				return quests
					.Where(q => id.Equals(q.Id))
					.Select(q => new QuestViewModel
					{
						Id = q.Id,

					});
			}
			return quests
				.Select(q => new QuestViewModel
				{
					Id = q.Id,
				});
		}

		[HttpPost("{id}/quests/{questId}")]
		[Consumes("application/json")]
		public async Task<QuestViewModel> SetQuestStatus(string id, string questId, [FromBody] QuestViewModel questStatus)
		{
			await _characterService.PerformAction(id, "do quest", questId);
			var quests = await _characterService.GetQuests(id);
			var quest = quests.FirstOrDefault(q => q.Id.Equals(questId));
			return new QuestViewModel
			{
				Id = quest.Id
			};
		}

		[HttpGet("{id}/actions")]
		[HttpGet("{id}/actions/{actionId?}")]
		public async Task<IEnumerable<ActionViewModel>> GetSupportedActions(string id, string actionId)
		{
			var actions = await _characterService.GetActions(id);
			if (string.IsNullOrEmpty(actionId))
			{
				return actions;
			}
			return actions.Where(a => a.Name.Equals(actionId));
		}

		[HttpPost("{id}/actions/{actionid}")]
		[Consumes("application/json")]
		public async Task<ActionViewModel> SetCurrentAction(string id, string actionId, [FromBody] ActionViewModel actionViewModel)
		{
			await _characterService.PerformAction(id, actionId, actionViewModel.Target);
			return actionViewModel;
		}
	}
}