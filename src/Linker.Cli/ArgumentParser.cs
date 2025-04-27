namespace Linker.Cli;

using Linker.Cli.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The command line argument parser.
/// </summary>
internal static class ArgumentParser
{
    private static readonly Dictionary<string, CommandType> CommandDictionary = new Dictionary<string, CommandType>
    {
        { "help", CommandType.Help },
        { "add", CommandType.AddLink },
        { "show", CommandType.ShowLinks },
        { "update", CommandType.UpdateLink },
        { "delete", CommandType.DeleteLink },
        { "visit", CommandType.VisitLink },
        { "search", CommandType.SearchLinks },
        { "list create", CommandType.CreateList },
        { "list show", CommandType.ShowLists },
        { "list update", CommandType.UpdateList },
        { "list add", CommandType.AddLinkIntoList },
        { "list remove", CommandType.RemoveLinkFromList },
        { "list delete", CommandType.DeleteList },
        { "export", CommandType.ExportLinks },
        { "get", CommandType.GetLink },
        { "list get", CommandType.GetList },
    };

    /// <summary>
    /// Parses the array of raw arguments into meaningful objects.
    /// </summary>
    /// <param name="args">The raw command line arguments.</param>
    /// <returns>A tuple consisting the type enum of command and the command object.</returns>
    /// <exception cref="NotImplementedException">Invalid command provided.</exception>
    public static (CommandType CommandType, object CommandArguments) Parse(string[] args)
    {
        CommandType commandType;

        if (args.FirstOrDefault(string.Empty).Equals("list"))
        {
            var verb = string.Join(' ', args[0], args[1]);
            commandType = CommandDictionary[verb];
        }
        else if (args.Length == 0 || args[0].Equals("--help") || args[0].Equals("-h"))
        {
            commandType = CommandDictionary["help"];
        }
        else
        {
            commandType = CommandDictionary[args[0]];
        }

        object command = commandType switch
        {
            CommandType.Help => new object(),
            CommandType.AddLink => ParseAddLinkCommand(args),
            CommandType.ShowLinks => ParseShowLinksCommand(args),
            CommandType.UpdateLink => ParseUpdateLinkCommand(args),
            CommandType.DeleteLink => ParseDeleteLinkCommand(args),
            CommandType.VisitLink => ParseVisitLinkCommand(args),
            CommandType.CreateList => ParseCreateListCommand(args),
            CommandType.UpdateList => ParseUpdateListCommand(args),
            CommandType.DeleteList => ParseDeleteListCommand(args),
            CommandType.ShowLists => ParseShowListsCommand(args),
            CommandType.AddLinkIntoList => ParseAddLinkIntoListCommand(args),
            CommandType.RemoveLinkFromList => ParseRemoveLinkFromListCommand(args),
            CommandType.SearchLinks => ParseSearchLinkCommands(args),
            CommandType.GetLink => ParseGetLinkCommand(args),
            CommandType.GetList => ParseGetListCommand(args),
            CommandType.ExportLinks => ParseExportLinksCommand(args),
            _ => throw new NotImplementedException(),
        };

        return (commandType, command);
    }

    private static AddLinkCommandArguments ParseAddLinkCommand(string[] args)
    {
        var index = 1;
        var command = new AddLinkCommandArguments
        {
            Url = args[index++],
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--name") || currentArgs.Equals("-n"))
            {
                command.Name = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--watch-later") || currentArgs.Equals("-w"))
            {
                command.WatchLater = true;
                index++;
            }
            else if (currentArgs.Equals("--tags") || currentArgs.Equals("-t"))
            {
                command.Tags = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--lang") || currentArgs.Equals("-l"))
            {
                command.Language = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static ShowLinksCommandArguments ParseShowLinksCommand(string[] args)
    {
        var index = 1;
        var command = new ShowLinksCommandArguments();

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--top") || currentArgs.Equals("-t"))
            {
                var value = int.Parse(args[index + 1]);
                command.Top = value == 0 ? null : value;
                index += 2;
            }
            else if (currentArgs.Equals("--watch-later") || currentArgs.Equals("-w"))
            {
                command.WatchLater = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else if (currentArgs.Equals("--all") || currentArgs.Equals("-a"))
            {
                command.All = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static UpdateLinkCommandArguments ParseUpdateLinkCommand(string[] args)
    {
        var index = 1;
        var command = new UpdateLinkCommandArguments
        {
            Id = int.Parse(args[index++]),
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--url") || currentArgs.Equals("-u"))
            {
                command.Url = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--name") || currentArgs.Equals("-n"))
            {
                command.Name = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--watch-later") || currentArgs.Equals("-w"))
            {
                command.WatchLater = true;
                index++;
            }
            else if (currentArgs.Equals("--no-watch-later") || currentArgs.Equals("-nw"))
            {
                command.NoWatchLater = true;
                index++;
            }
            else if (currentArgs.Equals("--tags") || currentArgs.Equals("-t"))
            {
                command.Tags = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--clear-tags") || currentArgs.Equals("-ct"))
            {
                command.ClearTags = true;
                index++;
            }
            else if (currentArgs.Equals("--add-tag") || currentArgs.Equals("-at"))
            {
                command.AddTags.Add(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--remove-tag") || currentArgs.Equals("-rt"))
            {
                command.RemoveTags.Add(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--lang") || currentArgs.Equals("-l"))
            {
                command.Language = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static DeleteLinkCommandArguments ParseDeleteLinkCommand(string[] args)
    {
        var index = 1;
        var command = new DeleteLinkCommandArguments
        {
            Id = int.Parse(args[index++]),
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--confirm") || currentArgs.Equals("-y"))
            {
                command.ConfirmDelete = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static VisitLinkCommandArguments ParseVisitLinkCommand(string[] args)
    {
        var index = 1;

        var command = new VisitLinkCommandArguments();

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--id") || currentArgs.Equals("-i"))
            {
                command.LinkId = int.Parse(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--random") || currentArgs.Equals("-r"))
            {
                command.Random = true;
                index++;
            }
            else if (currentArgs.Equals("--last") || currentArgs.Equals("-l"))
            {
                command.Last = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static CreateListCommandArguments ParseCreateListCommand(string[] args)
    {
        var index = 2;

        var command = new CreateListCommandArguments
        {
            Name = args[index++],
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static UpdateListCommandArguments ParseUpdateListCommand(string[] args)
    {
        var index = 2;

        var command = new UpdateListCommandArguments
        {
            Id = int.Parse(args[index++]),
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--name") || currentArgs.Equals("-n"))
            {
                command.Name = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static DeleteListCommandArguments ParseDeleteListCommand(string[] args)
    {
        var index = 2;
        var command = new DeleteListCommandArguments
        {
            Id = int.Parse(args[index++]),
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--confirm") || currentArgs.Equals("-y"))
            {
                command.ConfirmDelete = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static ShowListsCommandArguments ParseShowListsCommand(string[] args)
    {
        var index = 2;
        var command = new ShowListsCommandArguments();

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--top") || currentArgs.Equals("-t"))
            {
                command.Top = int.Parse(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--skip") || currentArgs.Equals("-s"))
            {
                command.Skip = int.Parse(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static AddLinkIntoListCommandArguments ParseAddLinkIntoListCommand(string[] args)
    {
        var index = 2;

        var command = new AddLinkIntoListCommandArguments
        {
            ListId = int.Parse(args[index++]),
            LinkId = int.Parse(args[index]),
        };

        return command;
    }

    private static RemoveLinkFromListCommandArguments ParseRemoveLinkFromListCommand(string[] args)
    {
        var index = 2;

        var command = new RemoveLinkFromListCommandArguments
        {
            ListId = int.Parse(args[index++]),
            LinkId = int.Parse(args[index]),
        };

        return command;
    }

    private static SearchLinkCommandArguments ParseSearchLinkCommands(string[] args)
    {
        var index = 1;

        // TODO: Try to make the first argument unnecessary for help command.
        var command = new SearchLinkCommandArguments
        {
            Keyword = args[index++],
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--top") || currentArgs.Equals("-t"))
            {
                command.Top = int.Parse(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--skip") || currentArgs.Equals("-s"))
            {
                command.Skip = int.Parse(args[index + 1]);
                index += 2;
            }
            else if (currentArgs.Equals("--tags") || currentArgs.Equals("-t"))
            {
                command.Tags = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static GetLinkCommandArguments ParseGetLinkCommand(string[] args)
    {
        var index = 1;

        var command = new GetLinkCommandArguments
        {
            Id = int.Parse(args[index++]),
        };

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--url") || currentArgs.Equals("-u"))
            {
                command.Url = true;
                index++;
            }
            else if (currentArgs.Equals("--name") || currentArgs.Equals("-n"))
            {
                command.Name = true;
                index++;
            }
            else if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = true;
                index++;
            }
            else if (currentArgs.Equals("--watch-later") || currentArgs.Equals("-w"))
            {
                command.WatchLater = true;
                index++;
            }
            else if (currentArgs.Equals("--tags") || currentArgs.Equals("-t"))
            {
                command.Tags = true;
                index++;
            }
            else if (currentArgs.Equals("--lang") || currentArgs.Equals("-l"))
            {
                command.Language = true;
                index++;
            }
            else if (currentArgs.Equals("--created-at") || currentArgs.Equals("-c"))
            {
                command.CreatedAt = true;
                index++;
            }
            else if (currentArgs.Equals("--modified-at") || currentArgs.Equals("-m"))
            {
                command.ModifiedAt = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static GetListCommandArguments ParseGetListCommand(string[] args)
    {
        var index = 2;

        var command = new GetListCommandArguments
        {
            Id = int.Parse(args[index++]),
        };

        while (index < args.Length)
        {
            var currentArgs = args[index++];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--name") || currentArgs.Equals("-n"))
            {
                command.Name = true;
                index++;
            }
            else if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = true;
                index++;
            }
            else if (currentArgs.Equals("--link") || currentArgs.Equals("-l"))
            {
                command.Links = true;
                index++;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }

    private static ExportLinksCommandArgument ParseExportLinksCommand(string[] args)
    {
        var index = 1;

        var command = new ExportLinksCommandArgument();

        while (index < args.Length)
        {
            var currentArgs = args[index];

            if (!currentArgs.StartsWith('-'))
            {
                throw new ArgumentException("Positional arguments must come before the optional arguments.");
            }

            if (currentArgs.Equals("--path") || currentArgs.Equals("-p"))
            {
                command.Path = args[index + 1];
                index += 2;
            }
            else if (currentArgs.Equals("--help") || currentArgs.Equals("-h"))
            {
                command.ShowHelp = true;
                index++;
            }
            else
            {
                throw new ArgumentException("Unrecognized args");
            }
        }

        return command;
    }
}
