using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Console.Interfaces
{
    public interface ICashCardService
    {
        Guid Create(string pin);
        void TopUp(Guid id, decimal amountToTopUpBy);
        void Withdraw(Guid id, string pin, decimal amountToWithDrawlBy);
        decimal GetBalance(Guid id);

        void InterCardTransfer(Guid cardA, string pin, Guid cardB, decimal amountToTransfer);
    }
}
