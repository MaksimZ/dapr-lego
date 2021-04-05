namespace Common.Services.Interfaces
{
    public interface ICharacterStoreFactory
    {
        ICharacterStore CreateCharacterStore(string characterId);
    }
}