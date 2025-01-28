using Dnd;
using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class AddGearHandler : IEventHandler<AddGearEvent>
{
    private readonly IKafkaMessageProducer<CharacterUpdateKey, CharacterUpdateValue> _producer;

    public AddGearHandler(IKafkaMessageProducer<CharacterUpdateKey, CharacterUpdateValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(AddGearEvent evt, CancellationToken cancellationToken)
    {
        var key = new CharacterUpdateKey()
        {
            GameId = evt.GameId,
        };

        var value = new CharacterUpdateValue
        {
            AddGear = new AddGearValue()
            {
                GameId = evt.GameId,
                CharacterId = evt.CharacterId,
                Gear = evt.Gear,
            },
        };
        var message = new KafkaProducerMessage<CharacterUpdateKey, CharacterUpdateValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}