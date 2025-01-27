using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application.Contracts.Events;

public record AddWeaponEvent(long GameId, long CharacterId, string Weapon) : IEvent;