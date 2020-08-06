namespace Pdb.DomainEvents.Abstractions
{
    using System.Threading.Tasks;

    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent);
    }
}