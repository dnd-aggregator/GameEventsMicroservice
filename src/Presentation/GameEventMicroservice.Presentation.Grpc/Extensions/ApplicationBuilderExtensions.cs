using GameEventMicroservice.Presentation.Grpc.Controllers;
using Microsoft.AspNetCore.Builder;

namespace GameEventMicroservice.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePresentationGrpc(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(routeBuilder =>
        {
            // TODO: add gRPC services implementation
            routeBuilder.MapGrpcService<GameStatusGrpcController>();
            routeBuilder.MapGrpcService<CharacterGrpcController>();

            // routeBuilder.MapGrpcService<Sample>();
            routeBuilder.MapGrpcReflectionService();
        });

        return builder;
    }
}