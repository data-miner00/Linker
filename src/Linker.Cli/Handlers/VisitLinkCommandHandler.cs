namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for visit link command.
/// </summary>
internal sealed class VisitLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> linkRepository;
    private readonly IRepository<Visit> visitRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="linkRepository">The link repository.</param>
    /// <param name="visitRepository">The visits repository.</param>
    public VisitLinkCommandHandler(IRepository<Link> linkRepository, IRepository<Visit> visitRepository)
    {
        this.linkRepository = Guard.ThrowIfNull(linkRepository);
        this.visitRepository = Guard.ThrowIfNull(visitRepository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is VisitLinkCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker visit <id> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --random            Visit a random link.");
                Console.WriteLine("  --last              Visit the last added link.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            var linkToVisit = await this.GetLinkFromArgs(args);

            var startInfo = new ProcessStartInfo
            {
                FileName = linkToVisit.Url,
                UseShellExecute = true,
            };

            Process.Start(startInfo);

            var visit = new Visit
            {
                LinkId = linkToVisit.Id,
                CreatedAt = DateTime.Now,
            };

            await this.visitRepository.AddAsync(visit);
            return;
        }

        throw new ArgumentException("Wrong args");
    }

    private async Task<Link> GetLinkFromArgs(VisitLinkCommandArguments args)
    {
        Link? linkToVisit;

        if (args.LinkId is not null)
        {
            linkToVisit = await this.linkRepository.GetByIdAsync(args.LinkId.Value);
        }
        else if (args.Random)
        {
            var links = (await this.linkRepository.GetAllAsync()).ToArray();

            var random = new Random();

            var randomIndex = random.Next(links.Length);

            linkToVisit = links[randomIndex];
        }
        else if (args.Last)
        {
            var links = (await this.linkRepository.GetAllAsync()).ToArray();

            linkToVisit = links.LastOrDefault();
        }
        else
        {
            throw new InvalidOperationException("No action can be taken with the current arguments.");
        }

        if (linkToVisit is null)
        {
            throw new InvalidOperationException("The link does not exist.");
        }

        return linkToVisit;
    }
}
