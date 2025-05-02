namespace Linker.Cli.Integrations.Serializers;

using Linker.Cli.Core.Serializers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using InternalSerializer = System.Text.Json.JsonSerializer;

/// <summary>
/// The JSON serializer for <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public sealed class JsonSerializer<T> : ISerializer<T>
    where T : class
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <inheritdoc/>
    public string Serialize(IEnumerable<T> items)
    {
        return InternalSerializer.Serialize(items, Options);
    }

    /// <inheritdoc/>
    public string Serialize(T item)
    {
        throw new NotImplementedException();
    }
}
