namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler to get a single link.
/// </summary>
internal sealed class GetLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public GetLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is GetLinkCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker get <id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --url               Show the URL.");
                Console.WriteLine("  --name              Show the name.");
                Console.WriteLine("  --description       Show the description.");
                Console.WriteLine("  --watch-later       Show if it's a watch later link.");
                Console.WriteLine("  --tags              Show the tags.");
                Console.WriteLine("  --language          Show the spoken language.");
                Console.WriteLine("  --created-at        Show the creation date.");
                Console.WriteLine("  --modified-at       Show the last modified date.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var link = await this.repository.GetByIdAsync(args.Id);

            if (link == null)
            {
                AnsiConsole.MarkupLine($"[red]The link with ID {args.Id} cannot be found.[/]");
                return;
            }

            bool[] flags = [
                args.Url,
                args.Name,
                args.Description,
                args.WatchLater,
                args.Tags,
                args.Language,
                args.CreatedAt,
                args.ModifiedAt,
            ];

            var isFlagsProvided = Array.Exists(flags, x => x);

            var table = new Table();
            table.AddColumns("Property", "Value");

            if (!isFlagsProvided || args.Url)
            {
                table.AddRow(new Markup("[bold]Url[/]"), new Text(link.Url));
            }

            if (!isFlagsProvided || args.Name)
            {
                table.AddRow("Name", link.Name ?? "-");
            }

            if (!isFlagsProvided || args.Description)
            {
                table.AddRow("Description", link.Description ?? "-");
            }

            if (!isFlagsProvided || args.WatchLater)
            {
                table.AddRow("Watch Later", link.WatchLater.ToString());
            }

            if (!isFlagsProvided || args.Tags)
            {
                table.AddRow("Tags", link.Tags ?? "-");
            }

            if (!isFlagsProvided || args.Language)
            {
                table.AddRow("Language", link.Language ?? "-");
            }

            if (!isFlagsProvided || args.CreatedAt)
            {
                table.AddRow("Created at", link.CreatedAt.ToString());
            }

            if (!isFlagsProvided || args.ModifiedAt)
            {
                table.AddRow("Modified at", link.ModifiedAt.ToString());
            }

            AnsiConsole.Write(table);

            return;
        }

        throw new ArgumentException("Invalid arguments");
    }
}
