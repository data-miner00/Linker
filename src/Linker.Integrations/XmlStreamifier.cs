namespace Linker.Integrations;

using Linker.Core.V2;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// The converter from object to Xml stream.
/// </summary>
public sealed class XmlStreamifier : IDataStreamifier
{
    /// <inheritdoc/>
    public Task<Stream> StreamifyAsync<T>(T @object, CancellationToken cancellationToken)
        where T : class
    {
        var xmlSerializer = new XmlSerializer(typeof(T));

        var stream = new MemoryStream();
        xmlSerializer.Serialize(stream, @object);

        // Reset the pointer
        stream.Seek(0, SeekOrigin.Begin);

        return Task.FromResult<Stream>(stream);
    }
}
