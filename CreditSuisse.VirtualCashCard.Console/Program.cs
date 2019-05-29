using CreditSuisse.VirtualCashCard.Console.Interfaces;
using CreditSuisse.VirtualCashCard.Console.Services;
using CreditSuisse.VirtualCashCard.Console.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CreditSuisse.VirtualCashCard.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddCustomLogging()
                .AddCQRS()
                .AddSingleton<ICashCardService, VirtualCashCardService>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            var service = serviceProvider.GetRequiredService<ICashCardService>();

            // creates a new Virtual Cash Card
            var id = service.Create("1234");
            
            // tops up the card by £100
            service.TopUp(id, 100.00M);

            // withdraws a tenner unsuccesfully
            service.Withdraw(id, "1235", 10.00M);

            // remembers pin number and withdraws a tenner
            service.Withdraw(id, "1234", 10.00M);

            // retrieves the balance of £90
            var balance = service.GetBalance(id);

            logger.LogInformation($"Balance of Card A: {balance:C}");

            var secondCardId = service.Create("4567");

            service.InterCardTransfer(id, "1234", secondCardId, 50.00M);

            // retrieves the balance of £40
            balance = service.GetBalance(id);

            logger.LogInformation($"Final Balance of Card A: {balance:C}");

            // retrieves the balance of £50
            balance = service.GetBalance(secondCardId);

            logger.LogInformation($"Final Balance of Card B: {balance:C}");

            logger.LogDebug("All done!");
        }
    }
}
