namespace Pdb.DomainEvents.Abstractions
{
    using System;

    public interface IDomainEvent
    {
        DateTimeOffset DateOccurred { get; }
    }
}