using GameEventMicroservice.Application.Contracts;
using GameEventMicroservice.Application.Contracts.Operations;
using Grpc.Core;
using System.Diagnostics;

namespace GameEventMicroservice.Presentation.Grpc.Controllers;

public class CharacterGrpcController : CharacterService.CharacterServiceBase
{
    private readonly ICharacterService _characterService;

    public CharacterGrpcController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    public override async Task<AddGearResponse> AddGear(AddGearRequest request, ServerCallContext context)
    {
        var subRequest = new AddGear.Request(request.GameId, request.CharacterId, request.Gear);
        AddGear.Result resp = await _characterService.AddGear(subRequest, context.CancellationToken);
        return resp switch
        {
            AddGear.Result.Success _ => new AddGearResponse(),

            AddGear.Result.NotFound invalidState => throw new RpcException(new Status(
                StatusCode.NotFound,
                "not found")),

            _ => throw new UnreachableException(),
        };
    }

    public override async Task<AddWeaponResponse> AddWeapon(AddWeaponRequest request, ServerCallContext context)
    {
        var subRequest = new AddWeapon.Request(request.GameId, request.CharacterId, request.Weapon);
        AddWeapon.Result resp = await _characterService.AddWeapon(subRequest, context.CancellationToken);
        return resp switch
        {
            AddWeapon.Result.Success _ => new AddWeaponResponse(),

            AddWeapon.Result.NotFound invalidState => throw new RpcException(new Status(
                StatusCode.NotFound,
                "not found")),

            _ => throw new UnreachableException(),
        };
    }

    public override async Task<KillCharacterResponse> KillCharacter(KillCharacterRequest request, ServerCallContext context)
    {
        var subRequest = new KillCharacter.Request(request.GameId, request.CharacterId);
        KillCharacter.Result resp = await _characterService.KillCharacter(subRequest, context.CancellationToken);
        return resp switch
        {
            KillCharacter.Result.Success _ => new KillCharacterResponse(),

            KillCharacter.Result.NotFound invalidState => throw new RpcException(new Status(
                StatusCode.NotFound,
                "not found")),

            _ => throw new UnreachableException(),
        };
    }
}