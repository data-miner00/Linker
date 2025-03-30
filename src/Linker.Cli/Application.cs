namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Handlers;
using Linker.Common.Helpers;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// The application orchestrator.
/// </summary>
internal sealed class Application
{
    private const int SUCCESS_CODE = 0;
    private const int FAILURE_CODE = 0x667;

    private readonly IDictionary<CommandType, Lazy<ICommandHandler>> commandHandlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    /// <param name="commandHandlers">The dictionary of command handlers.</param>
    public Application(IDictionary<CommandType, Lazy<ICommandHandler>> commandHandlers)
    {
        this.commandHandlers = Guard.ThrowIfNull(commandHandlers);
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
                DisplayHelpMessage();
            }
            else
            {
                if (this.commandHandlers.TryGetValue(command.CommandType, out var commandHandler))
                {
                    await commandHandler.Value.HandleAsync(arguments);
                }
            }

            return SUCCESS_CODE;
        }
        catch (KeyNotFoundException ex) when (ex.Message.StartsWith("The given key"))
        {
            Console.Error.WriteLine("The command could not be found.");
            return FAILURE_CODE;
        }
        catch (Exception ex)
        {
#if DEBUG
            Console.Error.WriteLine(ex.ToString());
#else
            Console.Error.WriteLine(ex.Message);
#endif
            return FAILURE_CODE;
        }
#if DEBUG
        finally
        {
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}ms", stopwatch.ElapsedMilliseconds);
        }
#endif
    }

    private static void DisplayHelpMessage()
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

        Console.WriteLine(prompt);
    }
}
