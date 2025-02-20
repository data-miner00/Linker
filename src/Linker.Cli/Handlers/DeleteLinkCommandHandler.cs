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

internal sealed class DeleteLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    public DeleteLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is DeleteLinkCommandArguments args)
        {
            if (!args.ConfirmDelete)
            {
                var linkToDelete = await this.repository.GetByIdAsync(args.Id);

                Console.Write($"Confirm delete {linkToDelete.Url}? [y/N]: ");
                var response = Console.ReadLine();

                if (response is null || !response.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Delete aborted.");
                    return;
                }
            }

            await this.repository.RemoveAsync(args.Id);
        }

        throw new ArgumentException("Invalid args");
    }
}
