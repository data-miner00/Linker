namespace Linker.Cli;

using Autofac;

internal static class Program
{
    internal static Task Main(string[] args)
    {
        var container = ContainerConfig.Configure();
        var app = container.Resolve<Application>();

        return app.ExecuteAsync(args);
    }
}
