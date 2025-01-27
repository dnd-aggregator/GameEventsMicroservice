namespace GameEventMicroservice.Application.Contracts.Operations;

public class AddGear
{
    public readonly record struct Request(long GameId, long CharacterId, string Gear);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success : Result;

        public sealed record NotFound : Result;
    }
}