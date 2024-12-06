namespace Linker.Integrations;

using Linker.Core.V2;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public sealed class JsonStreamifier : IDataStreamifier
{
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public JsonStreamifier()
    {
        this.jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };
    }

    public Task<Stream> StreamifyAsync(object @object, CancellationToken cancellationToken)
    {
        var jsonString = JsonSerializer.Serialize(@object, this.jsonSerializerOptions);
        var byteString = Encoding.UTF8.GetBytes(jsonString);

        var memoryStream = new MemoryStream(byteString);

        return Task.FromResult<Stream>(memoryStream);
    }
}
