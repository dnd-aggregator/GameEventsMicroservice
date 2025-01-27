using GameEventMicroservice.Application.Abstractions.Persistence;
using GameEventMicroservice.Application.Abstractions.Persistence.Repositories;

namespace GameEventMicroservice.Infrastructure.Persistence;

public class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(IGameStatusRepository gameStatusRepository)
    {
        GameStatusRepository = gameStatusRepository;
    }

    public IGameStatusRepository GameStatusRepository { get; }
}