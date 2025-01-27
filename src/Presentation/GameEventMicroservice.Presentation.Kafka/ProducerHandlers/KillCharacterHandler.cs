using Dnd;
using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class KillCharacterHandler : IEventHandler<CharacterKilledEvent>
{
    private readonly IKafkaMessageProducer<CharacterKilledKey, CharacterKilledValue> _producer;

    public KillCharacterHandler(IKafkaMessageProducer<CharacterKilledKey, CharacterKilledValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(CharacterKilledEvent evt, CancellationToken cancellationToken)
    {
        var key = new CharacterKilledKey()
        {
            GameId = evt.GameId,
        };

        var value = new CharacterKilledValue
        {
            GameId = evt.GameId,
            CharacterId = evt.CharacterId,
        };

        var message = new KafkaProducerMessage<CharacterKilledKey, CharacterKilledValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}