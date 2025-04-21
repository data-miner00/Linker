namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The delete list command handler.
/// </summary>
internal sealed class DeleteListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteListCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The url list repository.</param>
    public DeleteListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is DeleteListCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker list delete <list-id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --confirm           Confirm the deletion.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var listToDelete = await this.repository.GetByIdAsync(args.Id);

            if (listToDelete is null)
            {
                Console.WriteLine($"List with ID '{args.Id}' does not exist.");
                Environment.ExitCode = 1;
                return;
            }

            if (!args.ConfirmDelete)
            {
                Console.Write($"Confirm delete {listToDelete.Name}? [y/N]: ");
                var response = Console.ReadLine();

                if (response is null || !response.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Delete aborted.");
                    return;
                }
            }

            await this.repository.RemoveAsync(args.Id);
            return;
        }

        throw new ArgumentException("Args wrong");
    }
}
