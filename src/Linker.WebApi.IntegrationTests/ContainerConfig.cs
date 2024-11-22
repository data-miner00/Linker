namespace Linker.WebApi.IntegrationTests;

using Autofac;

internal static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        return builder.Build();
    }
}
