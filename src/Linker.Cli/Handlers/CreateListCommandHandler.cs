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

internal sealed class CreateListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    public CreateListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is CreateListCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker list create <name> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --description <desc> The description of the list.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var list = new UrlList
            {
                Name = args.Name,
                Description = args.Description,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };

            await this.repository.AddAsync(list);
            return;
        }

        throw new NotImplementedException();
    }
}
