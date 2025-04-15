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

internal sealed class UpdateListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    public UpdateListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
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
                Console.WriteLine($"The list with ID {args.Id} cannot be found.");
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
