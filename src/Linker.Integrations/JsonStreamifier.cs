namespace Linker.Integrations;

using Linker.Core.V2;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The converter from object to Json stream.
/// </summary>
public sealed class JsonStreamifier : IDataStreamifier
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    /// <inheritdoc/>
    public Task<Stream> StreamifyAsync<T>(T @object, CancellationToken cancellationToken)
        where T : class
    {
        var jsonString = JsonSerializer.Serialize(@object, this.jsonSerializerOptions);
        var byteString = Encoding.UTF8.GetBytes(jsonString);

        var memoryStream = new MemoryStream(byteString);

        return Task.FromResult<Stream>(memoryStream);
    }
}
