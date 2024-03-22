namespace Linker.Mvc.Models;

using System.Diagnostics.CodeAnalysis;

public class ErrorViewModel
{
    [NotNullIfNotNull(nameof(RequestId))]
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
