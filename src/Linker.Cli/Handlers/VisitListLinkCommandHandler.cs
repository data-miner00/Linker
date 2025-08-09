namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Extensions;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for visiting links in a list.
/// </summary>
internal sealed class VisitListLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Visit> visitRepository;
    private readonly IRepository<UrlList> listRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitListLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="visitRepository">The visit repository.</param>
    /// <param name="listRepository">The url list repository.</param>
    public VisitListLinkCommandHandler(
        IRepository<Visit> visitRepository,
        IRepository<UrlList> listRepository)
    {
        this.visitRepository = Guard.ThrowIfNull(visitRepository);
        this.listRepository = Guard.ThrowIfNull(listRepository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is not VisitListLinkCommandArguments args)
        {
            throw new ArgumentException($"Invalid command arguments");
        }

        if (args.ShowHelp)
        {
            Console.WriteLine("Usage: visit list <listId> [--all] [--random] [--last]");
            Console.WriteLine("Visit links in a list.");
            Console.WriteLine("Options:");
            Console.WriteLine("  --all                 Visit all links in the list.");
            Console.WriteLine("  --random              Visit a random link from the list.");
            Console.WriteLine("  --last                Visit the last link in the list.");
            Console.WriteLine("  --help                Show this help message.");
            return;
        }

        var list = await this.listRepository.GetByIdAsync(args.ListId);

        if (list == null)
        {
            AnsiConsole.MarkupLine("[red]List not found.[/]");
            return;
        }

        if (list.Links.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No links found in the list.[/]");
            return;
        }

        List<Link> linksToVisit = [];

        if (args.All)
        {
            linksToVisit.AddRange(list.Links);
        }
        else if (args.Random)
        {
            var randomLink = list.Links.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (randomLink != null)
            {
                linksToVisit.Add(randomLink);
            }
        }
        else if (args.Last)
        {
            var lastLink = list.Links.LastOrDefault();

            if (lastLink != null)
            {
                linksToVisit.Add(lastLink);
            }
        }

        List<Task> visitTasks = linksToVisit
            .Select(this.VisitLink)
            .ToList();

        await Task.WhenAll(visitTasks);
    }

    private Task VisitLink(Link link)
    {
        link.Url.VisitUrl();

        AnsiConsole.MarkupLine($"[green]Visited link:[/] {link.Url}");

        var visit = new Visit
        {
            LinkId = link.Id,
            CreatedAt = DateTime.UtcNow,
        };

        return this.visitRepository.AddAsync(visit);
    }
}
