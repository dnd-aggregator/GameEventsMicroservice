using GameEventMicroservice.Application.Abstractions.Persistence;
using GameEventMicroservice.Application.Contracts;
using GameEventMicroservice.Application.Contracts.Events;
using GameEventMicroservice.Application.Contracts.Operations;
using Itmo.Dev.Platform.Events;

namespace GameEventMicroservice.Application;

public class CharacterService : ICharacterService
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IEventPublisher _eventPublisher;

    public CharacterService(IPersistenceContext persistenceContext, IEventPublisher eventPublisher)
    {
        _persistenceContext = persistenceContext;
        _eventPublisher = eventPublisher;
    }

    public async Task<KillCharacter.Result> KillCharacter(
        KillCharacter.Request request,
        CancellationToken cancellationToken = default)
    {
        Models.Game? game = await _persistenceContext.GameStatusRepository.GetAsync(request.GameId, cancellationToken);
        if (game == null || game.CharactersIds.FirstOrDefault(ch => ch == request.CharacterId) == default)
        {
            return new KillCharacter.Result.NotFound();
        }

        await _eventPublisher.PublishAsync(new CharacterKilledEvent(request.GameId, request.CharacterId));
        return new KillCharacter.Result.Success();
    }

    public async Task<AddGear.Result> AddGear(AddGear.Request request, CancellationToken cancellationToken = default)
    {
        Models.Game? game = await _persistenceContext.GameStatusRepository.GetAsync(request.GameId, cancellationToken);
        if (game == null || game.CharactersIds.FirstOrDefault(ch => ch == request.CharacterId) == default)
        {
            return new AddGear.Result.NotFound();
        }

        await _eventPublisher.PublishAsync(new AddGearEvent(request.GameId, request.CharacterId, request.Gear));
        return new AddGear.Result.Success();
    }

    public async Task<AddWeapon.Result> AddWeapon(
        AddWeapon.Request request,
        CancellationToken cancellationToken = default)
    {
        Models.Game? game = await _persistenceContext.GameStatusRepository.GetAsync(request.GameId, cancellationToken);
        if (game == null || game.CharactersIds.FirstOrDefault(ch => ch == request.CharacterId) == default)
        {
            return new AddWeapon.Result.NotFound();
        }

        await _eventPublisher.PublishAsync(new AddWeaponEvent(request.GameId, request.CharacterId, request.Weapon));
        return new AddWeapon.Result.Success();
    }
}