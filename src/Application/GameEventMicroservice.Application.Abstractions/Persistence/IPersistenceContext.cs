using GameEventMicroservice.Application.Abstractions.Persistence.Repositories;

namespace GameEventMicroservice.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IGameStatusRepository GameStatusRepository { get; }
}