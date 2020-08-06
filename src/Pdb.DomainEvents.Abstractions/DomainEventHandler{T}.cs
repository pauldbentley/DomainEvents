namespace Pdb.DomainEvents.Abstractions
{
    using System.Threading.Tasks;

    public abstract class DomainEventHandler<T> : IDomainEventHandler<T>
         where T : IDomainEvent
    {
        public virtual int Order { get; } = 0;

        public abstract Task Handle(T domainEvent);
    }
}
