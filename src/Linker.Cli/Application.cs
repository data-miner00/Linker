namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Handlers;
using Linker.Common.Helpers;
using Spectre.Console;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// The application orchestrator.
/// </summary>
internal sealed class Application
{
    private const int SuccessCode = 0;
    private const int FailureCode = 0x667;

    private readonly IDictionary<CommandType, Lazy<ICommandHandler>> commandHandlers;
    private readonly IAnsiConsole console;

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    /// <param name="commandHandlers">The dictionary of command handlers.</param>
    /// <param name="console">The ansi console instance.</param>
    public Application(
        IDictionary<CommandType, Lazy<ICommandHandler>> commandHandlers,
        IAnsiConsole console)
    {
        this.commandHandlers = Guard.ThrowIfNull(commandHandlers);
        this.console = Guard.ThrowIfNull(console);
    }

    /// <summary>
    /// Executes the program.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The task.</returns>
    public async Task<int> ExecuteAsync(string[] args)
    {
#if DEBUG
        var stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        try
        {
            var command = ArgumentParser.Parse(args);
            var arguments = command.CommandArguments;

            if (command.CommandType == CommandType.Help)
            {
                this.DisplayHelpMessage();
            }
            else
            {
                if (this.commandHandlers.TryGetValue(command.CommandType, out var commandHandler))
                {
                    await commandHandler.Value.HandleAsync(arguments);
                }
                else
                {
                    this.console.MarkupLine($"[red]The command provided are not recognized.[/]");
                }
            }

            return SuccessCode;
        }
        catch (KeyNotFoundException ex) when (ex.Message.StartsWith("The given key"))
        {
            this.console.MarkupLine($"[red]The command could not be found.[/]");
            this.DisplayHelpMessage();

            return FailureCode;
        }
        catch (Exception ex)
        {
#if DEBUG
            this.console.WriteException(ex);
#else
            this.console.MarkupLine($"[red]{ex.Message}[/]");
#endif
            return FailureCode;
        }
#if DEBUG
        finally
        {
            stopwatch.Stop();
            this.console.MarkupLine("Time taken: [green]{0}ms[/]", stopwatch.ElapsedMilliseconds);
        }
#endif
    }

    private void DisplayHelpMessage()
    {
        var prompt = @"usage: linker <command> [options]

A CLI application to manage and organize links.

Commands:
  help                              Display this help message.
  add         <url> [options]       Add a new link. Optionally provide details.
  get         <id> [options]        Get a link. Optionally select details.
  show        [options]             Display all stored links.
  update      <id> [options]        Update an existing link's detail.
  delete      <id> [options]        Delete a link by its ID.
  visit       [options]             Open the specified link in the default browser.
  search      <keyword>             Search for links containing the specified keyword.
  list create <listname> [options]  Create a new list to organize links.
  list show   [options]             Display all created lists.
  list update <listid> [options]    Update details of an existing list.
  list add    <listid> <linkid>     Add a link to a specific list.
  list get    <listid> [options]    Gets a list. Optional select details.
  list remove <listid> <linkid>     Remove a link from a specific list.
  list delete <listid> [options]    Delete a list and all links within it.
  export      <filename> [options]  Export all links to a file in the specified format.

Options:
  -h, --help               Show help information.";

        this.console.WriteLine(prompt);
    }
}
