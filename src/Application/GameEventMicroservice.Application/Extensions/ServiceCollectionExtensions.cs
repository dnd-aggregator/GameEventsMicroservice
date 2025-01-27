using GameEventMicroservice.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace GameEventMicroservice.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        // TODO: add services
        collection.AddScoped<IGameStatusService, GameStatusService>();
        collection.AddScoped<ICharacterService, CharacterService>();
        return collection;
    }
}