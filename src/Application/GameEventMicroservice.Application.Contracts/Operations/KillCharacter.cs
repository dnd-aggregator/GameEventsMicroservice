namespace GameEventMicroservice.Application.Contracts.Operations;

public class KillCharacter
{
    public readonly record struct Request(long GameId, long CharacterId);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success : Result;

        public sealed record NotFound : Result;
    }
}