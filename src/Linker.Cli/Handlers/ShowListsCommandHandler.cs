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
/// The command handler for listing all lists.
/// </summary>
internal sealed class ShowListsCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowListsCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The list repository.</param>
    public ShowListsCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is ShowListsCommandArguments slca2)
        {
            var lists = await this.repository.GetAllAsync();

            if (lists.Any())
            {
                foreach (var (link, index) in lists
                    .SkipOrAll(slca2.Skip)
                    .TakeOrAll(slca2.Top)
                    .WithIndex())
                {
                    Console.WriteLine($"{index + 1}. {link.Name} - {link.Description}");
                }
            }
            else
            {
                Console.WriteLine("No lists has been added yet.");
            }

            return;
        }

        throw new ArgumentException("Args bad");
    }
}
