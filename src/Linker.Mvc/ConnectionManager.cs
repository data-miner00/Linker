namespace Linker.Mvc;

using Linker.Core.V2.Models;
using System.Collections.Concurrent;

public sealed class ConnectionManager
{
    public ConcurrentDictionary<string, ChatConnection> Connections { get; } = new();
}
