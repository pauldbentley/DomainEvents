namespace Pdb.DomainEvents
{
    using System.Threading.Tasks;

    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent);
    }
}