namespace Linker.Core.V2;

using System.Threading.Tasks;

/// <summary>
/// The abstraction for the data to stream converter.
/// </summary>
public interface IDataStreamifier
{
    /// <summary>
    /// Converts an object or a list of objects into a <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="T">Any reference type.</typeparam>
    /// <param name="object">The object to be streamified.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The converted stream.</returns>
    Task<Stream> StreamifyAsync<T>(T @object, CancellationToken cancellationToken)
        where T : class;
}
