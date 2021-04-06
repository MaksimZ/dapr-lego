using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CharacterApi.Controllers
{
	[ApiController]
	public class MessagesController : ControllerBase
	{

		private readonly ILogger _logger;
		public MessagesController(ILogger<MessagesController> logger)
		{
			_logger = logger;
		}
		[Topic("messages-channel", "self")]
		[HttpPost("self")]
		public async Task<IActionResult> MessageForSelf(Common.ActorInterfaces.Models.Message message)
		{
			await Task.Yield();
			_logger.LogInformation("{charid} said to self:  {message}", message.RecepientId, message.MessageText);
			return Ok();
		}

		[Topic("messages-channel", "personal")]
		[HttpPost("personal")]
		public async Task<IActionResult> MessageForChar(Common.ActorInterfaces.Models.Message message)
		{
			await Task.Yield();
			_logger.LogInformation("someone said to {charId}:  {message}", message.RecepientId, message.MessageText);
			return Ok();

		}

	}
}