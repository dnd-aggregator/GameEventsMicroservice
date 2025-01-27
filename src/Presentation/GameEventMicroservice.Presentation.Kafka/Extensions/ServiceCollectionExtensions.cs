using GameEventMicroservice.Presentation.Kafka.ConsumerHandlers;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameEventMicroservice.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string consumerKey = "Presentation:Kafka:Consumers";
        const string producerKey = "Presentation:Kafka:Producers";

        // TODO: add consumers and producers
        // consumer example:
        // .AddConsumer(b => b
        //     .WithKey<MessageKey>()
        //     .WithValue<MessageValue>()
        //     .WithConfiguration(configuration.GetSection($"{consumerKey}:MessageName"))
        //     .DeserializeKeyWithProto()
        //     .DeserializeValueWithProto()
        //     .HandleWith<MessageHandler>())
        //
        // producer example:
        // .AddProducer(b => b
        //     .WithKey<MessageKey>()
        //     .WithValue<MessageValue>()
        //     .WithConfiguration(configuration.GetSection($"{producerKey}:MessageName"))
        //     .SerializeKeyWithProto()
        //     .SerializeValueWithProto())
        collection.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddConsumer(b => b
                .WithKey<GameScheduleKey>()
                .WithValue<GameScheduleValue>()
                .WithConfiguration(configuration.GetSection($"{consumerKey}:MessageName"))
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .HandleInboxWith<GameStatusConsumerHandler>())
            .AddProducer(producer => producer
                .WithKey<GameStatusKey>()
                .WithValue<GameStatusValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:GameFinnish"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .WithOutbox())
            .AddProducer(producer => producer
                .WithKey<GameStatusKey>()
                .WithValue<GameStatusValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:GameStart"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .WithOutbox())
            .AddProducer(producer => producer
                .WithKey<CharacterKilledKey>()
                .WithValue<CharacterKilledValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:Killed"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .WithOutbox())
            .AddProducer(producer => producer
                .WithKey<AddWeaponKey>()
                .WithValue<AddWeaponValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:Weapon"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .WithOutbox())
            .AddProducer(producer => producer
                .WithKey<AddGearKey>()
                .WithValue<AddGearValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:Gear"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .WithOutbox()));

        return collection;
    }
}