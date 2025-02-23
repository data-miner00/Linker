namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for listing all links.
/// </summary>
internal sealed class ShowLinksCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowLinksCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public ShowLinksCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is ShowLinksCommandArguments args)
        {
            var links = await this.repository.GetAllAsync();

            if (links.Any())
            {
                foreach (var (link, index) in links
                    .SkipOrAll(args.Skip)
                    .TakeOrAll(args.Top)
                    .WithIndex())
                {
                    Console.WriteLine($"{index + 1}. {link.Url} - {link.Name}");
                }
            }
            else
            {
                Console.WriteLine("No links has been added yet.");
            }

            return;
        }

        throw new ArgumentException("Invalid arguments");
    }
}
