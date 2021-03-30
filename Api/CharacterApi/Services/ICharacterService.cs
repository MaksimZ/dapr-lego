using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Entities;

namespace CharacterApi.Services
{
	public interface ICharacterService
	{
        Task<IEnumerable<Quest>> GetQuests(string characterId);
        Task<IEnumerable<Location>> GetLocations(string characterId);
        Task<IEnumerable<Models.ActionViewModel>> GetActions(string characterId);
        Task<IEnumerable<Models.CharacterViewModel>> GetCharacters(string characterId);
        Task<Models.CharacterViewModel> GetSelf(string characterId);
        Task PerformAction(string action, string id);
	}
}