using GameEventMicroservice.Application.Abstractions.Persistence;
using GameEventMicroservice.Application.Contracts;
using GameEventMicroservice.Application.Contracts.Events;
using GameEventMicroservice.Application.Contracts.Operations;
using GameEventMicroservice.Application.Models;
using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application;

public class GameStatusService : IGameStatusService
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IEventPublisher _eventPublisher;

    public GameStatusService(IPersistenceContext persistenceContext, IEventPublisher eventPublisher)
    {
        _persistenceContext = persistenceContext;
        _eventPublisher = eventPublisher;
    }

    public async Task ScheduleGame(ScheduleGame game, CancellationToken cancellationToken)
    {
        var request = new Game(game.Id, GameStatus.Scheduled, game.CharactersIds);
        await _persistenceContext.GameStatusRepository.AddOrUpdateAsync([request], cancellationToken);
    }

    public async Task<StartGame.Result> StartGame(StartGame.Request game, CancellationToken cancellationToken = default)
    {
        Game? scheduledGame = await _persistenceContext.GameStatusRepository.GetAsync(game.Id, cancellationToken);
        if (scheduledGame == null) return new StartGame.Result.GameNotFound();

        var request = new Game(scheduledGame.Id, GameStatus.Started, scheduledGame.CharactersIds);
        await _persistenceContext.GameStatusRepository.AddOrUpdateAsync([request], cancellationToken);
        var gse = new GameStartedEvent(request.Id);
        await _eventPublisher.PublishAsync(gse, cancellationToken);
        return new StartGame.Result.Success();
    }

    public async Task<FinnishGame.Result> FinnishGame(
        FinnishGame.Request game,
        CancellationToken cancellationToken = default)
    {
        Game? scheduledGame = await _persistenceContext.GameStatusRepository.GetAsync(game.Id, cancellationToken);
        if (scheduledGame == null || scheduledGame.Status != GameStatus.Started)
            return new FinnishGame.Result.GameNotFound();

        var request = new Game(scheduledGame.Id, GameStatus.Finished, scheduledGame.CharactersIds);
        await _persistenceContext.GameStatusRepository.AddOrUpdateAsync([request], cancellationToken);
        var gfe = new GameFinnishedEvent(request.Id);
        await _eventPublisher.PublishAsync(gfe, cancellationToken);
        return new FinnishGame.Result.Success();
    }
}