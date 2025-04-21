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
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is ShowListsCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker list show [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --skip <number>     The number of lists to skip.");
                Console.WriteLine("  --top <number>      The number of lists to show.");
                Console.WriteLine("  --help             Show this help message.");
                return;
            }

            var lists = await this.repository.GetAllAsync();

            if (lists.Any())
            {
                foreach (var (link, index) in lists
                    .SkipOrAll(args.Skip)
                    .TakeOrAll(args.Top)
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
