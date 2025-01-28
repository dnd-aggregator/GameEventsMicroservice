using GameEventMicroservice.Application.Models;

namespace GameEventMicroservice.Application.Abstractions.Persistence.Repositories;

public interface IGameStatusRepository
{
    Task<Game?> GetAsync(long id, CancellationToken cancellationToken);

    Task AddAsync(Game game, CancellationToken cancellationToken);

    Task UpdateAsync(Game game, CancellationToken cancellationToken);
}