using Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Services.Interfaces
{
	public interface ILocationRepository
	{
		Task<Location> GetLocationAsync();
		Task<bool> SetLocationAsync(Location location);
		Task<IEnumerable<string>> GetCharactersInLocationAsync();
		Task<bool> StoreCharactersInLocationAsync(IEnumerable<string> charIds);
		Task<IEnumerable<string>> GetQuestsInLocationAsync();
		Task<bool> SetQuestsInLocationAsync(IEnumerable<string> quests);
	}
}