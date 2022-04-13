namespace Linker.ConsoleUI
{
    using Autofac;

    /// <summary>
    /// The entry class of the program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point of the program.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        internal static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using var scope = container.BeginLifetimeScope();
            var app = scope.Resolve<IStartup>();
            app.Run();
        }
    }
}
