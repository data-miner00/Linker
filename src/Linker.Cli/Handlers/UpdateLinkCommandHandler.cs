namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

internal sealed class UpdateLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    public UpdateLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is UpdateLinkCommandArguments args)
        {
            var original = await this.repository.GetByIdAsync(args.Id);

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

        throw new ArgumentException("What the");
    }
}
