namespace GameEventMicroservice.Application.Models;

public class Game
{
    public Game(long id, GameStatus status, IReadOnlyCollection<long> charactersIds)
    {
        Id = id;
        Status = status;
        CharactersIds = charactersIds;
    }

    public long Id { get; set; }

    public GameStatus Status { get; set; }

    public IReadOnlyCollection<long> CharactersIds { get; }
}