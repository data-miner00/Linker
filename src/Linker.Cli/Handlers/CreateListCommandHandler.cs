namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Threading.Tasks;

/// <summary>
/// The command handler for creating a new list.
/// </summary>
internal sealed class CreateListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;
    private readonly IAnsiConsole console;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateListCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The url list repository.</param>
    /// <param name="console">The ansi console instance.</param>
    public CreateListCommandHandler(IRepository<UrlList> repository, IAnsiConsole console)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.console = Guard.ThrowIfNull(console);
    }

    /// <inheritdoc/>
    public Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is not CreateListCommandArguments args)
        {
            throw new ArgumentException("Invalid arguments.");
        }

        if (args.ShowHelp)
        {
            var helpText = @"Usage: linker list create <name> [options]

Options:
  --description <desc> The description of the list.
  --help               Show this help message.
";

            this.console.WriteLine(helpText);
            return Task.CompletedTask;
        }

        var list = new UrlList
        {
            Name = args.Name,
            Description = args.Description,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };

        return this.repository.AddAsync(list);
    }
}
