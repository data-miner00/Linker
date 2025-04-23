namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for updating a list.
/// </summary>
internal sealed class UpdateListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateListCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The url list repository.</param>
    public UpdateListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is UpdateListCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker list update <list-id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --name <name>       The new name of the list.");
                Console.WriteLine("  --description <desc> The new description of the list.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var originalList = await this.repository.GetByIdAsync(args.Id);

            if (originalList == null)
            {
                AnsiConsole.MarkupLine($"[red]The list with ID '{args.Id}' cannot be found.[/]");
                Environment.ExitCode = 1;
                return;
            }

            if (args.Name is not null)
            {
                originalList.Name = args.Name;
            }

            if (args.Description is not null)
            {
                originalList.Description = args.Description;
            }

            await this.repository.UpdateAsync(originalList);

            return;
        }

        throw new NotImplementedException();
    }
}
