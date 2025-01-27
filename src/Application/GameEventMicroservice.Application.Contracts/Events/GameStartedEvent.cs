using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application.Contracts.Events;

public record GameStartedEvent(long Id) : IEvent;