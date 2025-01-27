namespace GameEventMicroservice.Application.Contracts.Operations;

public class ScheduleGame
{
    public long Id { get; set; }

    public IReadOnlyCollection<long> CharactersIds { get; }

    public ScheduleGame(long id, IReadOnlyCollection<long> charactersIds)
    {
        Id = id;
        CharactersIds = charactersIds;
    }
}