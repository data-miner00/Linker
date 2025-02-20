namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

internal sealed class ShowLinksCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    public ShowLinksCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is ShowLinksCommandArguments slca)
        {
            var links = await this.repository.GetAllAsync();

            foreach (var (index, link) in links
                .SkipOrAll(slca.Skip)
                .TakeOrAll(slca.Top)
                .Select((link, index) => (index, link)))
            {
                Console.WriteLine($"{index + 1}. {link.Url} - {link.Name}");
            }

            return;
        }

        throw new ArgumentException("Invalid arguments");
    }
}
