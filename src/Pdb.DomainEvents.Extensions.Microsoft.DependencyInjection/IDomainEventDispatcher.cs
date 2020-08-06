namespace Pdb.DomainEvents.Extensions.Microsoft.DependencyInjection
{
    using System.Threading.Tasks;
    using Pdb.DomainEvents.Abstractions;

    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent);
    }
}