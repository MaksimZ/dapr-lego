namespace Common.Services.Interfaces
{
    public interface IRepositoriesFactory
    {
        ICharacterRepository CreateCharacterRepository(string characterId);
        ILocationRepository CreateLocationRepository(string locationId);
        IQuestRepository CreateQuestRepository(string questId);
    }
}