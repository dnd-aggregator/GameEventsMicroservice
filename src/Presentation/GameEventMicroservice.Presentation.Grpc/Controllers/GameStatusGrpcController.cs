using Game;
using GameEventMicroservice.Application.Contracts;
using GameEventMicroservice.Application.Contracts.Operations;
using Grpc.Core;
using System.Diagnostics;

namespace GameEventMicroservice.Presentation.Grpc.Controllers;

public class GameStatusGrpcController : GameStatusService.GameStatusServiceBase
{
    private readonly IGameStatusService _gameStatusService;

    public GameStatusGrpcController(IGameStatusService gameStatusService)
    {
        _gameStatusService = gameStatusService;
    }

    public override async Task<StartGameResponse> StartGame(StartGameRequest request, ServerCallContext context)
    {
        var startRequest = new StartGame.Request()
        {
            Id = request.GameId,
        };
        StartGame.Result startResponce = await _gameStatusService.StartGame(startRequest);
        return startResponce switch
        {
            StartGame.Result.Success _ => new StartGameResponse(),

            StartGame.Result.GameNotFound invalidState => throw new RpcException(new Status(
                StatusCode.NotFound,
                "Order not found")),

            _ => throw new UnreachableException(),
        };
    }

    public override async Task<FinishGameResponse> FinishGame(FinishGameRequest request, ServerCallContext context)
    {
        var finnishRequest = new FinnishGame.Request(request.GameId);

        FinnishGame.Result startResponce = await _gameStatusService.FinnishGame(finnishRequest);
        return startResponce switch
        {
            FinnishGame.Result.Success => new FinishGameResponse(),

            FinnishGame.Result.GameNotFound invalidState => throw new RpcException(new Status(
                StatusCode.NotFound,
                "Order not found")),

            _ => throw new UnreachableException(),
        };
    }
}