namespace Pdb.DomainEvents
{
    using System.Threading.Tasks;

    public interface IDomainEventHandler<in T>
        where T : IDomainEvent
    {
        int Order { get; }

        Task Handle(T domainEvent);
    }
}