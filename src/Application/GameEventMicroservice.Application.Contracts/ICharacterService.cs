using GameEventMicroservice.Application.Contracts.Operations;

namespace GameEventMicroservice.Application.Contracts;

public interface ICharacterService
{
    public Task<KillCharacter.Result> KillCharacter(KillCharacter.Request request, CancellationToken cancellationToken);

    public Task<AddGear.Result> AddGear(AddGear.Request request, CancellationToken cancellationToken);

    public Task<AddWeapon.Result> AddWeapon(AddWeapon.Request request, CancellationToken cancellationToken);
}