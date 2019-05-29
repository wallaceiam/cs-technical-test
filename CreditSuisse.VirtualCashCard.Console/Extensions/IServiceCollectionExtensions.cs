using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.CommandHandlers;
using CreditSuisse.VirtualCashCard.Doman.Commands;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using CreditSuisse.VirtualCashCard.Console.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CreditSuisse.VirtualCashCard.Console.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.AddLogging(opts =>
             {
                 opts.SetMinimumLevel(LogLevel.Information);
                 opts.AddConsole();
             });

            return services;
        }

        public static IServiceCollection AddCQRS(this IServiceCollection services)
        {
            services
                .AddSingleton<IEventStore, InMemoryEventStore>()
                .AddSingleton<ICommandBus, InMemoryCommandBus>()
                .AddTransient<ICommandHandler<CreateCashCard>, CashCardCommandHandler>()
                .AddTransient<ICommandHandler<TopUpCashCard>, CashCardCommandHandler>()
                .AddTransient<ICommandHandler<WithdrawFromCashCard>, CashCardCommandHandler>();

            var provider = services.BuildServiceProvider();
            var commandSender = provider.GetRequiredService<ICommandBus>();
            commandSender.RegisterHandler<CreateCashCard>();
            commandSender.RegisterHandler<TopUpCashCard>();
            commandSender.RegisterHandler<WithdrawFromCashCard>();

            return services;
        }
    }
}
