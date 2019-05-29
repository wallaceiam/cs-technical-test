using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Interfaces
{
    public interface ICommandHandler<T> where T: ICommand
    {
        void Handle(T command);
    }
}
