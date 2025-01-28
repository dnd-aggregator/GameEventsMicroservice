using Dnd;
using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class AddWeaponHandler : IEventHandler<AddWeaponEvent>
{
    private readonly IKafkaMessageProducer<CharacterUpdateKey, CharacterUpdateValue> _producer;

    public AddWeaponHandler(IKafkaMessageProducer<CharacterUpdateKey, CharacterUpdateValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(AddWeaponEvent evt, CancellationToken cancellationToken)
    {
        var key = new CharacterUpdateKey()
        {
            GameId = evt.GameId,
        };

        var value = new CharacterUpdateValue
        {
            AddWeapon = new AddWeaponValue()
            {
                GameId = evt.GameId,
                CharacterId = evt.CharacterId,
                Weapon = evt.Weapon,
            },
        };
        var message = new KafkaProducerMessage<CharacterUpdateKey, CharacterUpdateValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}