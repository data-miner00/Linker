namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        { "list update", CommandType.UpdateList },
        { "list add", CommandType.AddLinkIntoList },
        { "list remove", CommandType.RemoveLinkFromList },
        { "list delete", CommandType.DeleteLink },
        { "export", CommandType.ExportLinks },
    };

    public static (CommandType CommandType, object CommandArguments) Parse(string[] args)
    {
        Guard.ThrowIfNullOrEmpty(args);

        CommandType commandType;
        object command;

        if (args.First().Equals("list"))
        {
            var verb = string.Join(' ', args[0], args[1]);
            commandType = CommandDictionary[verb];
        }
        else
        {
            commandType = CommandDictionary[args[0]];
        }

        if (commandType == CommandType.AddLink)
        {
            command = ParseAddLinkCommand(args);
        }
        else
        {
            throw new NotImplementedException();
        }

        return (commandType, command);
    }

    public static AddLinkCommandArguments ParseAddLinkCommand(string[] args)
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

            if (currentArgs.Equals("--description") || currentArgs.Equals("-d"))
            {
                command.Description = args[index + 1];
                index += 2;
            }

            if (currentArgs.Equals("--watch-later") || currentArgs.Equals("-w"))
            {
                command.WatchLater = true;
                index++;
            }

            if (currentArgs.Equals("--tags") || currentArgs.Equals("-t"))
            {
                command.Tags = args[index + 1];
                index += 2;
            }

            if (currentArgs.Equals("--lang") || currentArgs.Equals("-l"))
            {
                command.Language = args[index + 1];
                index += 2;
            }
        }

        return command;
    }
}

enum CommandType
{
    None,
    Help,
    AddLink,
    ShowLinks,
    UpdateLink,
    DeleteLink,
    VisitLink,
    SearchLinks,
    CreateList,
    UpdateList,
    AddLinkIntoList,
    RemoveLinkFromList,
    DeleteList,
    ExportLinks,
}
