using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class GameStartedHandler : IEventHandler<GameStartedEvent>
{
    private readonly IKafkaMessageProducer<GameStatusKey, GameStatusValue> _producer;

    public GameStartedHandler(IKafkaMessageProducer<GameStatusKey, GameStatusValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(GameStartedEvent evt, CancellationToken cancellationToken)
    {
        var key = new GameStatusKey()
        {
            GameId = evt.Id,
        };

        var value = new GameStatusValue
        {
            GameStarted = new GameStatusValue.Types.GameStarted()
            {
                GameId = evt.Id,
            },
        };

        var message = new KafkaProducerMessage<GameStatusKey, GameStatusValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}