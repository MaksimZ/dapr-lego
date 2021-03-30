using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Character = Entities.Character;
using Location = Entities.Location;
using Message = ActorInterfaces.Models.Message;
using Quest = Entities.Quest;


namespace ActorInterfaces
{
	public interface ICharacterActor
	{
        Task MoveTo(Location location);
        Task Attack(Character character);
        Task Speak(Message message);
        Task Observe();
        Task DoQuest(Quest quest);
        Task<IEnumerable<Quest>> GetKnownQuests();
        Task<IEnumerable<Location>> GetKnownLocations();
        Task<IEnumerable<Character>> GetKnownCharacters();
	}
}
