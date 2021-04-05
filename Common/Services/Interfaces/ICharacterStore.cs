using System.Collections.Generic;
using System.Threading.Tasks;
using Character = Common.Entities.Character;

namespace Common.Services.Interfaces
{
    public interface ICharacterStore
    {
        Task<Character> GetCharacterAsync();
        Task StoreCharacterAsync(Character character);
    }
}