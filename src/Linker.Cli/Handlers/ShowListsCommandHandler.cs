namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class ShowListsCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    public ShowListsCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is ShowListsCommandArguments slca2)
        {
            var lists = await this.repository.GetAllAsync();

            foreach (var (index, link) in lists
                .SkipOrAll(slca2.Skip)
                .TakeOrAll(slca2.Top)
                .Select((link, index) => (index, link)))
            {
                Console.WriteLine($"{index + 1}. {link.Name} - {link.Description}");
            }

            return;
        }

        throw new ArgumentException("Args bad");
    }
}
