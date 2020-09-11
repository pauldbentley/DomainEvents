namespace Pdb.DomainEvents
{
    using System;

    public interface IDomainEvent
    {
        DateTimeOffset DateOccurred { get; }
    }
}