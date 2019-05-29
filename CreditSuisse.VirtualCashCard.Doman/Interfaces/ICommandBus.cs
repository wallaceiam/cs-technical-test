using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Interfaces
{
    public interface ICommandBus
    {
        void RegisterHandler<T>() where T : ICommand;
        void Send<T>(T command) where T : ICommand;

    }
}
