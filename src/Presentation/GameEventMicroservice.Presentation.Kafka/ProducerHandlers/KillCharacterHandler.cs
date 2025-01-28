using Dnd;
using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class KillCharacterHandler : IEventHandler<CharacterKilledEvent>
{
    private readonly IKafkaMessageProducer<CharacterUpdateKey, CharacterUpdateValue> _producer;

    public KillCharacterHandler(IKafkaMessageProducer<CharacterUpdateKey, CharacterUpdateValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(CharacterKilledEvent evt, CancellationToken cancellationToken)
    {
        var key = new CharacterUpdateKey()
        {
            GameId = evt.GameId,
        };

        var value = new CharacterUpdateValue
        {
            CharacterKill = new CharacterKilledValue()
            {
                GameId = evt.GameId,
                CharacterId = evt.CharacterId,
            },
        };
        var message = new KafkaProducerMessage<CharacterUpdateKey, CharacterUpdateValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}