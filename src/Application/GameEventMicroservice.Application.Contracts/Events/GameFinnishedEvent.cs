using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application.Contracts.Events;

public record GameFinnishedEvent(long GameId) : IEvent;