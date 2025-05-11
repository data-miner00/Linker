namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Extensions;
using Linker.Common.Helpers;
using Spectre.Console;
using System;
using System.Threading.Tasks;

/// <summary>
/// The search list command handler.
/// </summary>
internal sealed class SearchListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchListCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The url list repository.</param>
    public SearchListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is SearchListCommandArguments args)
        {
            Guard.ThrowIfValidationFailed(args);

            if (args.ShowHelp)
            {
                Console.WriteLine("Usage: linker list search <keyword>");
                Console.WriteLine("Options:");
                Console.WriteLine("  --help              Show this help message.");

                return;
            }

            var lists = await this.repository.SearchAsync(args.Keyword);

            var table = new Table();
            table.AddColumns("No.", "ID", "Name", "Description", "Created At");

            foreach (var (list, index) in lists.WithIndex())
            {
                table.AddRow($"{index + 1}", $"{list.Id}", list.Name, list.Description ?? "-", list.CreatedAt.ToString());
            }

            AnsiConsole.Write(table);

            return;
        }

        throw new ArgumentException("Invalid args");
    }
}
