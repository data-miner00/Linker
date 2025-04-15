namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class DeleteListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    public DeleteListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
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
