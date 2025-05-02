namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Core.Serializers;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for exporting links.
/// </summary>
internal sealed class ExportLinksCommandHandler : ICommandHandler
{
    private const ExportFormat DefaultExportFormat = ExportFormat.Csv;

#if DEBUG
    private const string DefaultExportPath = "D:/staging";
#else
    private const string DefaultExportPath = "D:/export";
#endif
    private readonly IRepository<Link> repository;
    private readonly IDictionary<ExportFormat, Lazy<ISerializer<Link>>> serializers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportLinksCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    /// <param name="serializers">The link serializers.</param>
    public ExportLinksCommandHandler(
        IRepository<Link> repository,
        IDictionary<ExportFormat, Lazy<ISerializer<Link>>> serializers)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.serializers = Guard.ThrowIfNull(serializers);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is ExportLinksCommandArgument args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker export [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --path <path>           The path to export the links.");
                Console.WriteLine("  --format <format>       The format to export. Accepted csv or json.");
                Console.WriteLine("  --help                  Show this help message.");
                return;
            }

            var fileExtension = args.Format == ExportFormat.Json ? ".json" : ".csv";
            var exportPath = args.Path ?? DefaultExportPath + fileExtension;

            var links = await this.repository.GetAllAsync();

            if (!this.serializers.TryGetValue(args.Format ?? DefaultExportFormat, out var serializer))
            {
                throw new ArgumentException("Unsupported export format");
            }

            var serialized = serializer.Value.Serialize(links);

            File.WriteAllText(exportPath, serialized);

            Console.WriteLine("Content has been exported successfully to " + exportPath);

            return;
        }

        throw new ArgumentException("Bad arguments");
    }
}
