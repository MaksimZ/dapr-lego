using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Services.Interfaces;
using System;

namespace Api.QuestApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	[Consumes("application/json")]
	public class QuestController : ControllerBase
	{
		private readonly IRepositoriesFactory _repositoriesFactory;
		public QuestController(IRepositoriesFactory repositoriesFactory)
		{
			_repositoriesFactory = repositoriesFactory ?? throw new ArgumentNullException(nameof(repositoriesFactory));
		}
		// Get location
		[HttpGet("{questId}")]
		public async Task<IActionResult> GetQuest(string questId)
		{
			var locationRepo = _repositoriesFactory.CreateQuestRepository(questId);
			await Task.Yield();
			return Ok();
		}

		[HttpPost("{locationId}")]
		public async Task<IActionResult> CreateQuest(string questId, [FromBody] string questCreate)
		{
			var questRepo = _repositoriesFactory.CreateQuestRepository(questId);
			var location = new Common.Entities.Quest
			{
				Id = questId,
				Description = ""
			};
			await Task.WhenAll(
				//questRepo.SetLocationAsync(location)
			);

			return Ok();
		}
	}
}