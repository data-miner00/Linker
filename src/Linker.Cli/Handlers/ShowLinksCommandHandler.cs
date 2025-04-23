namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for listing all links.
/// </summary>
internal sealed class ShowLinksCommandHandler : ICommandHandler
{
    private readonly ILinkRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowLinksCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public ShowLinksCommandHandler(ILinkRepository repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is ShowLinksCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker show [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --watch-later        Show watch later links.");
                Console.WriteLine("  --top <number>       The number of links to show.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var links = await this.repository.GetAllAsync(args.WatchLater);

            if (links.Any())
            {
                var table = new Table();
                table.AddColumn("No.");
                table.AddColumn("ID");
                table.AddColumn("URL");
                table.AddColumn("Name");
                table.AddColumn("Tags");
                table.AddColumn("Created At");

                foreach (var (link, index) in links
                    .Reverse()
                    .TakeOrAll(args.All ? null : args.Top)
                    .WithIndex())
                {
                    table.AddRow($"{index + 1}", $"{link.Id}", link.Url, link.Name ?? "-", link.Tags ?? "-", link.CreatedAt.ToString());
                }

                AnsiConsole.Write(table);
            }
            else
            {
                Console.WriteLine("No links has been added yet.");
            }

            return;
        }

        throw new ArgumentException("Invalid arguments");
    }
}
