namespace Linker.Cli.Commands;

/// <summary>
/// The commands available for the CLI.
/// </summary>
internal enum CommandType
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
    ShowLists,
    UpdateList,
    AddLinkIntoList,
    RemoveLinkFromList,
    DeleteList,
    ExportLinks,
    GetLink,
    GetList,
    SearchLists,
    ExportLists,
}
