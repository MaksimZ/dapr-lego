using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Services.Interfaces;
using Api.LocationApi.Models;
using System;

namespace Api.LocationApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	[Consumes("application/json")]
	public class LocationController : ControllerBase
	{
		private readonly IRepositoriesFactory _repositoriesFactory;
		public LocationController(IRepositoriesFactory repositoriesFactory)
		{
			_repositoriesFactory = repositoriesFactory ?? throw new ArgumentNullException(nameof(repositoriesFactory));
		}
		// Get location
		[HttpGet("{locationId}")]
		public async Task<LocationResponseModel> GetLocation(string locationId)
		{
			var locationRepo = _repositoriesFactory.CreateLocationRepository(locationId);
			var location = await locationRepo.GetLocationAsync();

			return new LocationResponseModel
			{
				Id = locationId,
				Description = location.Description,
				ConnectedLocations = location.ConnectedLocations
			};
		}

		[HttpPost("{locationId}")]
		public async Task<LocationResponseModel> CreateLocation(string locationId, [FromBody] LocationCreateRequestModel locationCreate)
		{
			var locationRepo = _repositoriesFactory.CreateLocationRepository(locationId);
			var location = new Common.Entities.Location
			{
				Id = locationId,
				ConnectedLocations = locationCreate.ConnectedLocations,
				Description = locationCreate.Description
			};
			await Task.WhenAll(
				locationRepo.SetLocationAsync(location),
				locationRepo.SetQuestsInLocationAsync(locationCreate.QuestsInLocation)
			);

			return new LocationResponseModel
			{
				Id = locationId,
				ConnectedLocations = location.ConnectedLocations,
				Description = location.Description
			};
		}

		[HttpGet("{locationId}/characters")]
		public async Task<IEnumerable<string>> GetCharactersInLocation(string locationId)
		{
			var locationRepo = _repositoriesFactory.CreateLocationRepository(locationId);
			return await locationRepo.GetCharactersInLocationAsync();
		}

		[HttpGet("{locationId}/quests")]
		public async Task<IEnumerable<string>> GetQuestsInLocations(string locationId)
		{
			var locationRepo = _repositoriesFactory.CreateLocationRepository(locationId);
			return await locationRepo.GetQuestsInLocationAsync();
		}
	}
}