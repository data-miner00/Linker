namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for deleting a link.
/// </summary>
internal sealed class DeleteLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public DeleteLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is DeleteLinkCommandArguments args)
        {
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
                Console.WriteLine($"Link with ID {args.Id} not found.");
                return;
            }

            if (!args.ConfirmDelete)
            {
                Console.Write($"Confirm delete {linkToDelete.Url}? [y/N]: ");
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

        throw new ArgumentException("Invalid args");
    }
}
