using GameEventMicroservice.Application.Contracts;
using GameEventMicroservice.Application.Contracts.Operations;
using Itmo.Dev.Platform.Kafka.Consumer;

namespace GameEventMicroservice.Presentation.Kafka.ConsumerHandlers;

public class GameStatusConsumerHandler : IKafkaInboxHandler<GameScheduleKey, GameScheduleValue>
{
    private readonly IGameStatusService _gameStatusService;

    public GameStatusConsumerHandler(IGameStatusService gameStatusService)
    {
        _gameStatusService = gameStatusService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<GameScheduleKey, GameScheduleValue>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<GameScheduleKey, GameScheduleValue> message in messages)
        {
            var request = new ScheduleGame(message.Value.GameId, message.Value.CharacterIds);

            await _gameStatusService.ScheduleGame(request, cancellationToken);
        }
    }
}