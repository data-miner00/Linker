namespace Linker.Core.V2.ApiModels;

public sealed record CreateChatMessage
{
    public string AuthorId { get; set; }

    public string WorkspaceId { get; set; }

    public string Content { get; set; }
}
