namespace Linker.Core.V2.ApiModels;

using System.ComponentModel.DataAnnotations;

public sealed class UpdateUserRequest
{
    [EmailAddress]
    public string Email { get; set; }

    public string PhotoUrl { get; set; }

    public string Description { get; set; }
}
