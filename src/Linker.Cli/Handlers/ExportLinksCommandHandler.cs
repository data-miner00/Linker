namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Core.Serializers;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

internal sealed class ExportLinksCommandHandler : ICommandHandler
{
#if DEBUG
    private const string DefaultExportPath = "D:/staging.csv";
#else
    private const string DefaultExportPath = "D:/export.csv";
#endif
    private readonly IRepository<Link> repository;
    private readonly ISerializer<Link> serializer;

    public ExportLinksCommandHandler(IRepository<Link> repository, ISerializer<Link> serializer)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.serializer = Guard.ThrowIfNull(serializer);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is ExportLinksCommandArgument args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker export [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --path <path>       The path to export the links.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var exportPath = args.Path ?? DefaultExportPath;

            var links = await this.repository.GetAllAsync();

            var serialized = this.serializer.Serialize(links);

            File.WriteAllText(exportPath, serialized);

            Console.WriteLine("Content has been exported successfully to " + exportPath);

            return;
        }

        throw new ArgumentException("Bad arguments");
    }
}
