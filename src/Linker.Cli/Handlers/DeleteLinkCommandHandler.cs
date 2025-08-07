namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for deleting a link.
/// </summary>
internal sealed class DeleteLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;
    private readonly IAnsiConsole console;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    /// <param name="console">The ansi console instance.</param>
    public DeleteLinkCommandHandler(IRepository<Link> repository, IAnsiConsole console)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.console = Guard.ThrowIfNull(console);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is not DeleteLinkCommandArguments args)
        {
            throw new ArgumentException("Invalid args");
        }

        if (args.ShowHelp)
        {
            Console.WriteLine("Usage: linker delete <link-id> [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  --confirm           Confirm the deletion.");
            Console.WriteLine("  --help              Show this help message.");
            return;
        }

        var linkToDelete = await this.repository.GetByIdAsync(args.Id);

        if (linkToDelete is null)
        {
            this.console.MarkupLineInterpolated($"[red]Link with ID \"{args.Id}\" could not be found.[/]");
            return;
        }

        if (!args.ConfirmDelete)
        {
            this.console.MarkupInterpolated($"[yellow]Confirm delete {linkToDelete.Url}? [[y/N]]: [/]");
            var response = Console.ReadLine();

            if (response is null || !response.StartsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                this.console.MarkupLine("[red]Delete aborted.[/]");
                return;
            }
        }

        await this.repository.RemoveAsync(args.Id);
        this.console.MarkupLine("[green]Successfully deleted the link.[/]");
    }
}
