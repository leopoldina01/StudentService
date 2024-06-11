//kod preuzet iz materijala sa vezbi
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Observer
{
    interface ISubject
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
    }
}
