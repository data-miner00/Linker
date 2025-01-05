namespace Linker.Core.V2.Events;

using System;

public sealed class LinkUpdatedEvent
{
    public int Id { get; set; }

    public Guid LinkId { get; set; }

    public DateTime EnqueuedAt { get; set; }
}
