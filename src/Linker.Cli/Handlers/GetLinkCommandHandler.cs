namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler to get a single link.
/// </summary>
internal sealed class GetLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public GetLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is GetLinkCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker get <id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --url               Show the URL.");
                Console.WriteLine("  --name              Show the name.");
                Console.WriteLine("  --description       Show the description.");
                Console.WriteLine("  --watch-later       Show if it's a watch later link.");
                Console.WriteLine("  --tags              Show the tags.");
                Console.WriteLine("  --language          Show the spoken language.");
                Console.WriteLine("  --created-at        Show the creation date.");
                Console.WriteLine("  --modified-at       Show the last modified date.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var link = await this.repository.GetByIdAsync(args.Id);

            if (link == null)
            {
                Console.WriteLine($"The link with ID {args.Id} cannot be found.");
                return;
            }

            bool[] flags = [
                args.Url,
                args.Name,
                args.Description,
                args.WatchLater,
                args.Tags,
                args.Language,
                args.CreatedAt,
                args.ModifiedAt,
            ];

            var isFlagsProvided = Array.Exists(flags, x => x);

            if (!isFlagsProvided || args.Url)
            {
                Console.WriteLine("Url: " + link.Url);
            }

            if (!isFlagsProvided || args.Name)
            {
                Console.WriteLine("Name: " + link.Name);
            }

            if (!isFlagsProvided || args.Description)
            {
                Console.WriteLine("Description: " + link.Description);
            }

            if (!isFlagsProvided || args.WatchLater)
            {
                Console.WriteLine("Watch Later: " + link.WatchLater.ToString());
            }

            if (!isFlagsProvided || args.Tags)
            {
                Console.WriteLine("Tags: " + link.Tags);
            }

            if (!isFlagsProvided || args.Language)
            {
                Console.WriteLine("Language: " + link.Language);
            }

            if (!isFlagsProvided || args.CreatedAt)
            {
                Console.WriteLine("Created at: " + link.CreatedAt);
            }

            if (!isFlagsProvided || args.ModifiedAt)
            {
                Console.WriteLine("Modified at: " + link.ModifiedAt);
            }

            return;
        }

        throw new ArgumentException("Invalid arguments");
    }
}
