﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Character = Common.Entities.Character;
using Location = Common.Entities.Location;
using Message = Common.ActorInterfaces.Models.Message;
using Quest = Common.Entities.Quest;
using Dapr.Actors;


namespace Common.ActorInterfaces
{
	public interface ICharacterActor: IActor
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
