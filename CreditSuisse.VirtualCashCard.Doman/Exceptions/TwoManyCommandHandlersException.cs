using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Exceptions
{
    public class TwoManyCommandHandlersException : Exception
    {
        public TwoManyCommandHandlersException(string message)
            :base(message)
        {

        }
    }
}
