using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class GameFinishedHandler : IEventHandler<GameFinnishedEvent>
{
    private readonly IKafkaMessageProducer<GameStatusKey, GameStatusValue> _producer;

    public GameFinishedHandler(IKafkaMessageProducer<GameStatusKey, GameStatusValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(GameFinnishedEvent evt, CancellationToken cancellationToken)
    {
        var key = new GameStatusKey()
        {
            GameId = evt.GameId,
        };

        var value = new GameStatusValue
        {
            GameFinished = new GameStatusValue.Types.GameFinished()
            {
                GameId = evt.GameId,
            },
        };

        var message = new KafkaProducerMessage<GameStatusKey, GameStatusValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}