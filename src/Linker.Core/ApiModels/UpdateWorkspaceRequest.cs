namespace Linker.Core.ApiModels;

public sealed class UpdateWorkspaceRequest
{
    required public string Handle { get; set; }

    required public string Name { get; set; }

    required public string Description { get; set; }
}
