namespace Pdb.DomainEvents.Abstractions
{
    using System;

    public abstract class DomainEvent : IDomainEvent
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}