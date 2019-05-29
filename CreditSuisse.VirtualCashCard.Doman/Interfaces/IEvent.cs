using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Interfaces
{
    public interface IEvent
    {
        int Version { get; }
        void SetVersion(int version);
    }
}
