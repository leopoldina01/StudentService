using Projekat.Observer;
using Projekat.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model.DAO
{
    public class AdresaDAO : ISubject
    {
        private readonly List<IObserver> _observers;

        private readonly AdresaStorage _storage;
        private readonly List<Adresa> _adrese;

        public AdresaDAO()
        {
            _storage = new AdresaStorage();
            _adrese = _storage.Load();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            if (_adrese.Count == 0)
                return 1;
            return _adrese.Max(s => s.Id) + 1;
        }

        public void Add(string ulica, string broj, string grad, string drzava)
        {
            int id = NextId();

            Adresa Adresa = new Adresa(id, ulica, broj, grad, drzava);
            _adrese.Add(Adresa);
            _storage.Save(_adrese);
            NotifyObservers();
        }

        public void Remove(Adresa adresa)
        {
            _adrese.Remove(adresa);
            _storage.Save(_adrese);
            NotifyObservers();
        }

        public void Save()
        {
            _storage.Save(_adrese);
        }

        public Adresa getAdresaById(int id)
        {
            return _adrese.Find(a => a.Id == id);
        }

        public List<Adresa> GetAll()
        {
            return _adrese;
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
