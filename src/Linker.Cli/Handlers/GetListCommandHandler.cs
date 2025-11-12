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
/// The command handler for getting a list.
/// </summary>
internal sealed class GetListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetListCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The url list repository.</param>
    public GetListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(object commandArguments)
    {
        Guard.ThrowIfNull(commandArguments);

        if (commandArguments is not GetListCommandArguments args)
        {
            throw new ArgumentException("Invalid args");
        }

        Guard.ThrowIfValidationFailed(args);

        if (args.ShowHelp)
        {
            Console.WriteLine("Usage: linker list get <list-id> [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  --links            Show links in the list.");
            Console.WriteLine("  --name             Show name of the list.");
            Console.WriteLine("  --description      Show description of the list.");
            Console.WriteLine("  --help             Show this help message.");
            return;
        }

        var list = await this.repository.GetByIdAsync(args.Id)
            ?? throw new InvalidOperationException($"The link with ID '{args.Id}' cannot be found.");

        bool[] flags = [
            args.Links,
            args.Name,
            args.Description,
        ];

        var isFlagsProvided = Array.Exists(flags, x => x);

        var grid = new Grid();

        grid.AddColumn(new GridColumn());
        if (!isFlagsProvided || args.Name)
        {
            var panel = new Panel(new Text(Markup.Escape(list.Name)).Centered());
            panel.Expand = true;
            panel.Border = BoxBorder.Double;

            grid.AddRow(panel);
        }

        if ((!isFlagsProvided || args.Description) && list.Description != null)
        {
            var panel = new Panel(new Text(Markup.Escape(list.Description)).Centered());
            panel.Expand = true;

            grid.AddRow(panel);
        }

        if (!isFlagsProvided || args.Links)
        {
            var table = new Table();
            table.AddColumn("No.");
            table.AddColumn("ID");
            table.AddColumn("URL");
            table.AddColumn("Name");
            table.AddColumn("Tags");
            table.AddColumn("Created At");

            foreach (var (link, index) in list.Links.WithIndex())
            {
                table.AddRow($"{index + 1}", $"{link.Id}", link.Url, Markup.Escape(link.Name ?? "-"), link.Tags ?? "-", link.CreatedAt.ToString());
            }

            grid.AddRow(table);
        }

        AnsiConsole.Write(grid);
    }
}
