namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for removing a link from a list.
/// </summary>
internal sealed class RemoveLinkFromListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> listRepository;
    private readonly IRepository<Link> linkRepository;
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveLinkFromListCommandHandler"/> class.
    /// </summary>
    /// <param name="listRepository">The url list repository.</param>
    /// <param name="linkRepository">The link repository.</param>
    /// <param name="context">The DB context.</param>
    public RemoveLinkFromListCommandHandler(
        IRepository<UrlList> listRepository,
        IRepository<Link> linkRepository,
        AppDbContext context)
    {
        this.listRepository = Guard.ThrowIfNull(listRepository);
        this.linkRepository = Guard.ThrowIfNull(linkRepository);
        this.context = Guard.ThrowIfNull(context);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is RemoveLinkFromListCommandArguments args)
        {
            var list = await this.listRepository.GetByIdAsync(args.ListId);
            var link = await this.linkRepository.GetByIdAsync(args.LinkId);

            if (list is null)
            {
                throw new InvalidOperationException($"List with ID '{args.ListId}' does not exist.");
            }
            else if (link is null)
            {
                throw new InvalidOperationException($"Link with ID '{args.LinkId}' does not exist.");
            }

            var isSuccess = list.Links.Remove(link);

            if (isSuccess)
            {
                await this.context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Link with ID '{args.LinkId}' is not in the list with ID '{args.ListId}'.");
            }

            return;
        }

        throw new ArgumentException("Invalid args");
    }
}
