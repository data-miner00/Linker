﻿namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

internal sealed class AddLinkIntoListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> listRepository;
    private readonly IRepository<Link> linkRepository;
    private readonly AppDbContext context;

    public AddLinkIntoListCommandHandler(
        IRepository<UrlList> listRepository,
        IRepository<Link> linkRepository,
        AppDbContext context)
    {
        this.listRepository = Guard.ThrowIfNull(listRepository);
        this.linkRepository = Guard.ThrowIfNull(linkRepository);
        this.context = Guard.ThrowIfNull(context);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is AddLinkIntoListCommandArguments args)
        {
            var list = await this.listRepository.GetByIdAsync(args.ListId);
            var link = await this.linkRepository.GetByIdAsync(args.LinkId);

            list.Links.Add(link);
            await this.context.SaveChangesAsync();

            return;
        }

        throw new ArgumentException("Invalid args");
    }
}
