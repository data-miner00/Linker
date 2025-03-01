namespace Linker.Cli;

using Autofac;

/// <summary>
/// The Linker CLI program.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Entry point for the program.
    /// </summary>
    /// <param name="args">A list of command line arguments.</param>
    /// <returns>The task.</returns>
    internal static Task Main(string[] args)
    {
        var container = ContainerConfig.Configure();
        var app = container.Resolve<Application>();

        return app.ExecuteAsync(args);
    }
}
