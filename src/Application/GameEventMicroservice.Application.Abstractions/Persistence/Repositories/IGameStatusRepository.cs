using GameEventMicroservice.Application.Models;

namespace GameEventMicroservice.Application.Abstractions.Persistence.Repositories;

public interface IGameStatusRepository
{
    Task AddOrUpdateAsync(IReadOnlyCollection<Game> games, CancellationToken cancellationToken);

    Task<Game?> GetAsync(long id, CancellationToken cancellationToken);
}