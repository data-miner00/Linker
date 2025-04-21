namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for updating a link.
/// </summary>
internal sealed class UpdateLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public UpdateLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is UpdateLinkCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker update <link-id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --url <url>        Update the URL of the link.");
                Console.WriteLine("  --name <name>      Update the name of the link.");
                Console.WriteLine("  --description <desc> Update the description of the link.");
                Console.WriteLine("  --watch-later       Mark the link as watch later.");
                Console.WriteLine("  --no-watch-later    Unmark the link as watch later.");
                Console.WriteLine("  --tags <tags>      Add tags to the link.");
                Console.WriteLine("  --clear-tags       Clear all tags from the link.");
                Console.WriteLine("  --add-tags <tags>  Add tags to the link.");
                Console.WriteLine("  --remove-tags <tags> Remove tags from the link.");
                Console.WriteLine("  --language <lang>  Update the language of the link.");
                Console.WriteLine("  --help             Show this help message.");
                return;
            }

            var original = await this.repository.GetByIdAsync(args.Id)
                ?? throw new InvalidOperationException($"The link with ID '{args.Id}' cannot be found.");

            if (args.Url is not null)
            {
                original.Url = args.Url;
            }

            if (args.Name is not null)
            {
                original.Name = args.Name;
            }

            if (args.Description is not null)
            {
                original.Description = args.Description;
            }

            if (args.WatchLater)
            {
                original.WatchLater = true;
            }

            if (args.NoWatchLater)
            {
                original.WatchLater = false;
            }

            if (args.Tags is not null)
            {
                original.Tags = args.Tags;
            }

            if (args.ClearTags)
            {
                original.Tags = null;
            }

            if (args.AddTags.Count > 0)
            {
                var combined = string.Join(',', args.AddTags);
                if (original.Tags is not null)
                {
                    original.Tags = string.Join(',', original.Tags, combined);
                }
                else
                {
                    original.Tags = combined;
                }
            }

            if (args.RemoveTags.Count > 0 && original.Tags is not null) // prob write a warning here
            {
                string[] tempSplitted = original.Tags.Split(',');
                var filteredTags = tempSplitted.Where(tag => !args.RemoveTags.Contains(tag)).ToArray();
                original.Tags = string.Join(',', filteredTags);
            }

            if (args.Language is not null)
            {
                original.Language = args.Language;
            }

            original.ModifiedAt = DateTime.Now;

            await this.repository.UpdateAsync(original);

            return;
        }

        throw new ArgumentException("Invalid args");
    }
}
