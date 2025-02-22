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

internal sealed class SearchLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    public SearchLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is SearchLinkCommandArguments args)
        {
            var links = await this.repository.SearchAsync(args.Keyword);

            foreach (var (index, link) in links
                .SkipOrAll(args.Skip)
                .TakeOrAll(args.Top)
                .Select((link, index) => (index, link)))
            {
                Console.WriteLine($"{index + 1}. {link.Url} - {link.Name}");
            }

            return;
        }

        throw new ArgumentException("Not supported args");
    }
}
