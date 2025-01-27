using Dnd;
using GameEventMicroservice.Application.Contracts.Events;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace GameEventMicroservice.Presentation.Kafka.ProducerHandlers;

public class AddWeaponHandler : IEventHandler<AddWeaponEvent>
{
    private readonly IKafkaMessageProducer<AddWeaponKey, AddWeaponValue> _producer;

    public AddWeaponHandler(IKafkaMessageProducer<AddWeaponKey, AddWeaponValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(AddWeaponEvent evt, CancellationToken cancellationToken)
    {
        var key = new AddWeaponKey()
        {
            GameId = evt.GameId,
        };

        var value = new AddWeaponValue
        {
            GameId = evt.GameId,
            CharacterId = evt.CharacterId,
            Weapon = evt.Weapon,
        };

        var message = new KafkaProducerMessage<AddWeaponKey, AddWeaponValue>(key, value);
        await _producer.ProduceAsync(message, cancellationToken);
    }
}