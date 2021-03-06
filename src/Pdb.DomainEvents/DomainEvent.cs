﻿namespace Pdb.DomainEvents
{
    using System;
    using System.Threading.Tasks;

    public abstract class DomainEvent : IDomainEvent
    {
        public static Func<IDomainEventDispatcher> Dispatcher { get; set; }

        public DateTimeOffset DateOccurred { get; set; } = DateTime.Now;

        public static async Task Raise<TDomainEvent>(TDomainEvent args)
            where TDomainEvent : IDomainEvent
        {
            var dispatcher = Dispatcher.Invoke();
            await dispatcher.Dispatch(args).ConfigureAwait(false);
        }
    }
}
