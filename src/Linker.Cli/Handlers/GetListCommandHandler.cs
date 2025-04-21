namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for getting a list.
/// </summary>
internal sealed class GetListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetListCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The url list repository.</param>
    public GetListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is GetListCommandArguments args)
        {
            Guard.ThrowIfValidationFailed(args);

            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker list get <list-id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --links            Show links in the list.");
                Console.WriteLine("  --name             Show name of the list.");
                Console.WriteLine("  --description      Show description of the list.");
                Console.WriteLine("  --help             Show this help message.");
                return;
            }

            var list = await this.repository.GetByIdAsync(args.Id)
                ?? throw new InvalidOperationException($"The link with ID '{args.Id}' cannot be found.");

            bool[] flags = [
                args.Links,
                args.Name,
                args.Description,
            ];

            var isFlagsProvided = Array.Exists(flags, x => x);

            if (!isFlagsProvided || args.Name)
            {
                Console.WriteLine("Name: " + list.Name);
            }

            if (!isFlagsProvided || args.Description)
            {
                Console.WriteLine("Description: " + list.Description);
            }

            if (!isFlagsProvided || args.Links)
            {
                Console.WriteLine("Links:");

                foreach (var link in list.Links)
                {
                    Console.WriteLine($"{link.Id}. {link.Url} - {link.Name}");
                }
            }

            return;
        }

        throw new ArgumentException("Invalid args");
    }
}
