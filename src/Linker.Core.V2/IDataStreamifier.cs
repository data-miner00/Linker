namespace Linker.Core.V2;

using System.Threading.Tasks;

public interface IDataStreamifier
{
    Task<Stream> StreamifyAsync(object @object, CancellationToken cancellationToken);
}
