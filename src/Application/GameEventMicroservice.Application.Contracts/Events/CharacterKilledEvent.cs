using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application.Contracts.Events;

public record CharacterKilledEvent(long GameId, long CharacterId) : IEvent;