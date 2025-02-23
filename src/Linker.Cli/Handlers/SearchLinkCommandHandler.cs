﻿namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The command handler for searching links.
/// </summary>
internal sealed class SearchLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public SearchLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is SearchLinkCommandArguments args)
        {
            var links = await this.repository.SearchAsync(args.Keyword);

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
                Console.WriteLine($"Cannot find results with '{args.Keyword}' keyword.");
            }

            return;
        }

        throw new ArgumentException("Not supported args");
    }
}
