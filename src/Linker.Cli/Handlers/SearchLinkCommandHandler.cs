namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for searching links.
/// </summary>
internal sealed class SearchLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public SearchLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is SearchLinkCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker search <keyword> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --tags              Search by tags.");
                Console.WriteLine("  --skip <number>     The number of links to skip.");
                Console.WriteLine("  --top <number>      The number of links to show.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            Link[]? links;

            if (!args.Tags)
            {
                links = (await this.repository.SearchAsync(args.Keyword)).ToArray();
                Console.WriteLine($"Found {links.Length} results for keyword '{args.Keyword}'.");
            }
            else
            {
                links = (await this.repository.GetAllAsync())
                    .Where(x => x.Tags is not null && x.Tags.Contains(args.Keyword))
                    .ToArray();
                Console.WriteLine($"Found {links.Length} results for tag '{args.Keyword}'.");
            }

            if (links.Length == 0)
            {
                return;
            }

            Console.WriteLine();

            var table = new Table();
            table.AddColumns("No.", "ID", "URL", "Name", "Tags", "Created At");

            foreach (var (link, index) in links
                .SkipOrAll(args.Skip)
                .TakeOrAll(args.Top)
                .WithIndex())
            {
                table.AddRow($"{index + 1}", $"{link.Id}", link.Url, link.Name ?? "-", link.Tags ?? "-", link.CreatedAt.ToString());
            }

            AnsiConsole.Write(table);

            return;
        }

        throw new ArgumentException("Not supported args");
    }
}
