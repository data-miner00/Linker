namespace Linker.Core.V2.Clients;

using Linker.Core.V2.Events;
using System.Threading.Tasks;

public interface ILinkUpdatedEventClient
{
    Task PublishAsync(string linkId, CancellationToken cancellationToken);

    Task<LinkUpdatedEvent> PeekAsync(CancellationToken cancellationToken);

    Task<IEnumerable<LinkUpdatedEvent>> PeekBatchAsync(CancellationToken cancellationToken);

    Task CompleteAsync(long id, CancellationToken cancellationToken);
}
