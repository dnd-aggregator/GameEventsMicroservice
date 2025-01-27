namespace GameEventMicroservice.Application.Contracts.Operations;

public class AddWeapon
{
    public readonly record struct Request(long GameId, long CharacterId, string Weapon);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success : Result;

        public sealed record NotFound : Result;
    }
}