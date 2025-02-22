namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Handlers;
using System.Threading.Tasks;

/// <summary>
/// The application orchestrator.
/// </summary>
internal sealed class Application
{
    private readonly AddLinkCommandHandler addLinkCommandHandler;
    private readonly ShowLinksCommandHandler showLinksCommandHandler;
    private readonly UpdateLinkCommandHandler updateLinkCommandHandler;
    private readonly DeleteLinkCommandHandler deleteLinkCommandHandler;
    private readonly VisitLinkCommandHandler visitLinkCommandHandler;
    private readonly CreateListCommandHandler createListCommandHandler;
    private readonly ShowListsCommandHandler showListsCommandHandler;
    private readonly UpdateListCommandHandler updateListCommandHandler;
    private readonly DeleteListCommandHandler deleteListCommandHandler;
    private readonly AddLinkIntoListCommandHandler addLinkIntoListCommandHandler;
    private readonly RemoveLinkFromListCommandHandler removeLinkFromListCommandHandler;
    private readonly SearchLinkCommandHandler searchLinkCommandHandler;

    public Application(
        AddLinkCommandHandler addLinkCommandHandler,
        ShowLinksCommandHandler showLinksCommandHandler,
        UpdateLinkCommandHandler updateLinkCommandHandler,
        DeleteLinkCommandHandler deleteLinkCommandHandler,
        VisitLinkCommandHandler visitLinkCommandHandler,
        CreateListCommandHandler createListCommandHandler,
        ShowListsCommandHandler showListsCommandHandler,
        UpdateListCommandHandler updateListCommandHandler,
        DeleteListCommandHandler deleteListCommandHandler,
        AddLinkIntoListCommandHandler addLinkIntoListCommandHandler,
        RemoveLinkFromListCommandHandler removeLinkFromListCommandHandler,
        SearchLinkCommandHandler searchLinkCommandHandler)
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
    }

    /// <summary>
    /// Executes the program.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The task.</returns>
    public async Task ExecuteAsync(string[] args)
    {
        var command = ArgumentParser.Parse(args);
        var arguments = command.CommandArguments;

        switch (command.CommandType)
        {
            case CommandType.Help:
                DisplayHelpMessage();
                break;
            case CommandType.AddLink:
                await this.addLinkCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.ShowLinks:
                await this.showLinksCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.UpdateLink:
                await this.updateLinkCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.DeleteLink:
                await this.deleteLinkCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.VisitLink:
                await this.visitLinkCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.CreateList:
                await this.createListCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.ShowLists:
                await this.showListsCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.UpdateList:
                await this.updateListCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.DeleteList:
                await this.deleteListCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.AddLinkIntoList:
                await this.addLinkIntoListCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.RemoveLinkFromList:
                await this.removeLinkFromListCommandHandler.HandleAsync(arguments);
                break;
            case CommandType.SearchLinks:
                await this.searchLinkCommandHandler.HandleAsync(arguments);
                break;
        }
    }

    internal static void DisplayHelpMessage()
    {
        var prompt = @"usage: linker <command> [options]

A CLI application to manage and organize links.

Commands:
  help                              Display this help message.
  add         <url> [options]       Add a new link. Optionally provide details.
  show        [options]             Display all stored links.
  update      <id> [options]        Update an existing link's detail.
  delete      <id> [options]        Delete a link by its ID.
  visit       [options]             Open the specified link in the default browser.
  search      <keyword>             Search for links containing the specified keyword.
  list create <listname> [options]  Create a new list to organize links.
  list show   [options]             Display all created lists.
  list update <listid> [options]    Update details of an existing list.
  list add    <listid> <linkid>     Add a link to a specific list.
  list remove <listid> <linkid>     Remove a link from a specific list.
  list delete <listid> [options]    Delete a list and all links within it.
  export      <filename> [options]  Export all links to a file in the specified format.

Options:
  -h, --help               Show help information.";

        Console.WriteLine(prompt);
        /*
            animation
            color
            link --help
            link add https://www.what.com --name blah --description blah --watch-later --tags tag1,tag2,tag3 --lang en
            link show --top 10 --skip 20
            link update 1 --name blah --url https://updated.what.com --description blah --clear-tags --add-tag tag1 --remove-tag tag2 --lang kr
            link delete 1
            link visit 1
            link search nixos
            link list show
            link list create 'my list' --description hello
            link list update 1 --name 'my 2 list' --description hello
            link list add 1 1
            link list remove 1 1 --confirm
            link list delete 1 --confirm
            link export --format csv,json,xml --destination home.csv
         */
    }
}
