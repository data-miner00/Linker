namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AddLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> repository;

    public AddLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = repository;
    }

    public Task HandleAsync(object commandArguments)
    {
        if (commandArguments is AddLinkCommandArguments args)
        {
            return this.repository.AddAsync(args.ToLink());
        }

        throw new ArgumentException("The arguments provided does not match the command.");
    }
}
