using CreditSuisse.VirtualCashCard.Doman.Exceptions;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CreditSuisse.VirtualCashCard.Console.Infrastructure
{
    public class InMemoryCommandBus : ICommandBus
    {
        private static readonly Dictionary<Type, Action<ICommand>> routes = new Dictionary<Type, Action<ICommand>>();
        private readonly IServiceProvider provider;
        public InMemoryCommandBus(IServiceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public void RegisterHandler<T>() where T : ICommand
        {           
            if (routes.ContainsKey(typeof(T)))
            {
                throw new TwoManyCommandHandlersException($"Command {typeof(T)} already has a command handler registered");
            }

            var handler = this.provider.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;
            routes.Add(typeof(T), x => handler.Handle((T)x));
        }

        public void Send<T>(T command) where T : ICommand
        {
            if (!routes.TryGetValue(typeof(T), out var handler))
            {
                throw new NoCommandHandlersRegisteredException();
            }

            // execute the handler
            handler(command);
        }
    }
}
