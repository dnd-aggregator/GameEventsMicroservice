using GameEventMicroservice.Application.Contracts.Operations;

namespace GameEventMicroservice.Application.Contracts;

public interface IGameStatusService
{
    public Task ScheduleGame(ScheduleGame game, CancellationToken cancellationToken = default);

    public Task<StartGame.Result> StartGame(StartGame.Request game, CancellationToken cancellationToken = default);

    public Task<FinnishGame.Result> FinnishGame(FinnishGame.Request game, CancellationToken cancellationToken = default);
}