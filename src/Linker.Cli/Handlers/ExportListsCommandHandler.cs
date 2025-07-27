namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Core.Serializers;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler to export lists.
/// </summary>
internal sealed class ExportListsCommandHandler : ICommandHandler
{
    private const ExportFormat DefaultExportFormat = ExportFormat.Csv;

    private readonly IRepository<UrlList> listRepository;
    private readonly IDictionary<ExportFormat, Lazy<ISerializer<Link>>> linkSerializer;
    private readonly IDictionary<ExportFormat, Lazy<ISerializer<UrlList>>> listSerializer;
    private readonly string defaultExportPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportListsCommandHandler"/> class.
    /// </summary>
    /// <param name="listRepository">The url list repository.</param>
    /// <param name="linkSerializer">The link serializer.</param>
    /// <param name="listSerializer">The url list serializer.</param>
    /// <param name="defaultExportPath">The default export path.</param>
    public ExportListsCommandHandler(
        IRepository<UrlList> listRepository,
        IDictionary<ExportFormat, Lazy<ISerializer<Link>>> linkSerializer,
        IDictionary<ExportFormat, Lazy<ISerializer<UrlList>>> listSerializer,
        string defaultExportPath)
    {
        this.listRepository = listRepository;
        this.linkSerializer = linkSerializer;
        this.listSerializer = listSerializer;
        this.defaultExportPath = defaultExportPath;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is not ExportListsCommandArguments args)
        {
            throw new ArgumentException("Bad arguments");
        }

        if (args.ShowHelp)
        {
            Console.WriteLine("Usage: linker list export [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  --path <path>           The path to export the lists.");
            Console.WriteLine("  --format <format>       The format to export. Accepted csv or json.");
            Console.WriteLine("  --listId <listId>       The links inside a specific list to export.");
            Console.WriteLine("  --help                  Show this help message.");
            return;
        }

        var isSpecificList = args.ListId.HasValue;

        if (isSpecificList)
        {
            await this.ExportSpecificList(args);
        }
        else
        {
            await this.ExportAllLists(args);
        }
    }

    private async Task ExportAllLists(ExportListsCommandArguments args)
    {
        var lists = await this.listRepository.GetAllAsync();

        if (!this.listSerializer.TryGetValue(args.Format ?? DefaultExportFormat, out var serializer))
        {
            throw new ArgumentException("Unsupported export format.");
        }

        var serialized = serializer.Value.Serialize(lists);

        var fileExtension = args.Format == ExportFormat.Json ? ".json" : ".csv";
        var exportPath = args.Path ?? this.defaultExportPath + fileExtension;

        File.WriteAllText(exportPath, serialized);

        AnsiConsole.MarkupLine($"[green]Exported successfully.[/]");
    }

    private async Task ExportSpecificList(ExportListsCommandArguments args)
    {
        var list = await this.listRepository.GetByIdAsync(args.ListId!.Value)
            ?? throw new InvalidOperationException("List does not exist."); ;

        var links = list.Links.ToList();

        if (!this.linkSerializer.TryGetValue(args.Format ?? DefaultExportFormat, out var serializer))
        {
            throw new ArgumentException("Unsupported export format.");
        }

        var serialized = serializer.Value.Serialize(links);

        var fileExtension = args.Format == ExportFormat.Json ? ".json" : ".csv";
        var specificList = "-" + args.ListId.Value;
        var exportPath = args.Path ?? this.defaultExportPath + specificList + fileExtension;

        File.WriteAllText(exportPath, serialized);

        AnsiConsole.MarkupLine($"[green]Exported single list successfully.[/]");
    }
}
