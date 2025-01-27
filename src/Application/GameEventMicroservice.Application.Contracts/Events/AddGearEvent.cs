using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application.Contracts.Events;

public record AddGearEvent(long GameId, long CharacterId, string Gear) : IEvent;