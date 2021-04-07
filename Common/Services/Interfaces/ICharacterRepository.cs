using System.Collections.Generic;
using System.Threading.Tasks;
using Character = Common.Entities.Character;

namespace Common.Services.Interfaces
{
    public interface ICharacterRepository
    {
        Task<Character> GetCharacterAsync();
        Task<bool> StoreCharacterAsync(Character character);
    }
}