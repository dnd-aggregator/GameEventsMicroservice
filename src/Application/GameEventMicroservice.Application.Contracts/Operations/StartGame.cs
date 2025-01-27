namespace GameEventMicroservice.Application.Contracts.Operations;

public static class StartGame
{
    public readonly record struct Request(long Id);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success : Result;

        public sealed record GameNotFound : Result;
    }
}