namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Handlers;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// The application orchestrator.
/// </summary>
internal sealed class Application
{
    private readonly Lazy<AddLinkCommandHandler> addLinkCommandHandler;
    private readonly Lazy<ShowLinksCommandHandler> showLinksCommandHandler;
    private readonly Lazy<UpdateLinkCommandHandler> updateLinkCommandHandler;
    private readonly Lazy<DeleteLinkCommandHandler> deleteLinkCommandHandler;
    private readonly Lazy<VisitLinkCommandHandler> visitLinkCommandHandler;
    private readonly Lazy<CreateListCommandHandler> createListCommandHandler;
    private readonly Lazy<ShowListsCommandHandler> showListsCommandHandler;
    private readonly Lazy<UpdateListCommandHandler> updateListCommandHandler;
    private readonly Lazy<DeleteListCommandHandler> deleteListCommandHandler;
    private readonly Lazy<AddLinkIntoListCommandHandler> addLinkIntoListCommandHandler;
    private readonly Lazy<RemoveLinkFromListCommandHandler> removeLinkFromListCommandHandler;
    private readonly Lazy<SearchLinkCommandHandler> searchLinkCommandHandler;
    private readonly Lazy<GetLinkCommandHandler> getLinkCommandHandler;
    private readonly Lazy<GetListCommandHandler> getListCommandHandler;
    private readonly Lazy<ExportLinksCommandHandler> exportLinksCommandHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    public Application(
        Lazy<AddLinkCommandHandler> addLinkCommandHandler,
        Lazy<ShowLinksCommandHandler> showLinksCommandHandler,
        Lazy<UpdateLinkCommandHandler> updateLinkCommandHandler,
        Lazy<DeleteLinkCommandHandler> deleteLinkCommandHandler,
        Lazy<VisitLinkCommandHandler> visitLinkCommandHandler,
        Lazy<CreateListCommandHandler> createListCommandHandler,
        Lazy<ShowListsCommandHandler> showListsCommandHandler,
        Lazy<UpdateListCommandHandler> updateListCommandHandler,
        Lazy<DeleteListCommandHandler> deleteListCommandHandler,
        Lazy<AddLinkIntoListCommandHandler> addLinkIntoListCommandHandler,
        Lazy<RemoveLinkFromListCommandHandler> removeLinkFromListCommandHandler,
        Lazy<SearchLinkCommandHandler> searchLinkCommandHandler,
        Lazy<GetLinkCommandHandler> getLinkCommandHandler,
        Lazy<GetListCommandHandler> getListCommandHandler,
        Lazy<ExportLinksCommandHandler> exportLinksCommandHandler)
    {
        this.addLinkCommandHandler = addLinkCommandHandler;
        this.showLinksCommandHandler = showLinksCommandHandler;
        this.updateLinkCommandHandler = updateLinkCommandHandler;
        this.deleteLinkCommandHandler = deleteLinkCommandHandler;
        this.visitLinkCommandHandler = visitLinkCommandHandler;
        this.createListCommandHandler = createListCommandHandler;
        this.showListsCommandHandler = showListsCommandHandler;
        this.updateListCommandHandler = updateListCommandHandler;
        this.deleteListCommandHandler = deleteListCommandHandler;
        this.addLinkIntoListCommandHandler = addLinkIntoListCommandHandler;
        this.removeLinkFromListCommandHandler = removeLinkFromListCommandHandler;
        this.searchLinkCommandHandler = searchLinkCommandHandler;
        this.getLinkCommandHandler = getLinkCommandHandler;
        this.getListCommandHandler = getListCommandHandler;
        this.exportLinksCommandHandler = exportLinksCommandHandler;
    }

    /// <summary>
    /// Executes the program.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The task.</returns>
    public async Task ExecuteAsync(string[] args)
    {
#if DEBUG
        var stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        try
        {
            var command = ArgumentParser.Parse(args);
            var arguments = command.CommandArguments;

            switch (command.CommandType)
            {
                case CommandType.Help:
                    DisplayHelpMessage();
                    break;
                case CommandType.AddLink:
                    await this.addLinkCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.ShowLinks:
                    await this.showLinksCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.UpdateLink:
                    await this.updateLinkCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.DeleteLink:
                    await this.deleteLinkCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.VisitLink:
                    await this.visitLinkCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.CreateList:
                    await this.createListCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.ShowLists:
                    await this.showListsCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.UpdateList:
                    await this.updateListCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.DeleteList:
                    await this.deleteListCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.AddLinkIntoList:
                    await this.addLinkIntoListCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.RemoveLinkFromList:
                    await this.removeLinkFromListCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.SearchLinks:
                    await this.searchLinkCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.GetLink:
                    await this.getLinkCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.GetList:
                    await this.getListCommandHandler.Value.HandleAsync(arguments);
                    break;
                case CommandType.ExportLinks:
                    await this.exportLinksCommandHandler.Value.HandleAsync(arguments);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
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
