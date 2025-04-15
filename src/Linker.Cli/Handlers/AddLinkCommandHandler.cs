namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for adding new <see cref="Link"/>.
/// </summary>
internal sealed class AddLinkCommandHandler : ICommandHandler
{
    private const string UniqueConstraintFailedMessage = "SQLite Error 19: 'UNIQUE constraint failed: Links.Url'";

    private readonly IRepository<Link> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddLinkCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The link repository.</param>
    public AddLinkCommandHandler(IRepository<Link> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is AddLinkCommandArguments args)
        {
            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker add <url> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --name <name>        The name of the link.");
                Console.WriteLine("  --description <desc> The description of the link.");
                Console.WriteLine("  --watch-later        Add the link to watch later.");
                Console.WriteLine("  --tags <tags>       Comma-separated list of tags.");
                Console.WriteLine("  --language <lang>   The spoken language.");
                Console.WriteLine("  --help              Show this help message.");
                return;
            }

            try
            {
                await this.repository.AddAsync(args.ToLink());
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null && ex.InnerException.Message.Contains(UniqueConstraintFailedMessage))
                {
                    Console.Error.WriteLine("The URL already exists. Please add a new one.");
                    Environment.ExitCode = 1;
                    return;
                }

                throw;
            }
        }

        throw new ArgumentException("The arguments provided does not match the command.");
    }
}
