using Microsoft.Win32;
using Projekat.Controller;
using Projekat.Manager.Serialization;
using Projekat.Observer;
using Projekat.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projekat.Model.DAO
{
    class KatedraDAO : ISubject
    {

        private readonly string StoragePathProfesori = string.Format("..{0}..{0}..{0}Data{0}profesori.csv", Path.DirectorySeparatorChar);
        private readonly Serializer<Profesor> _serializer;

        private ProfesorController _profesori;
        private readonly List<IObserver> _observers;

        private readonly KatedraStorage _storage;
        private readonly List<Katedra> _katedre;

        public KatedraDAO()
        {
            _storage = new KatedraStorage();
            _katedre = _storage.Load();
            _observers = new List<IObserver>();

            _serializer = new Serializer<Profesor>();
        }

        public void LoadComplexData(ProfesorController profesori)
        {
            foreach (Katedra katedra in _katedre)
            {
                Profesor profesor = profesori.GetAllProfesors().Find(p => p.Id == katedra.sefId);
                katedra.sef = profesor;
            }
            NotifyObservers();
        }

        public void SetSefKatedre(Katedra katedra, Profesor profesor, ProfesorController profesorController, Profesor stariSef)
        {
            katedra.sef = profesor;
            katedra.sefId = profesor.Id;
            /*if (stariSef != null)
            {
                stariSef.idKatedre = -1;
            }*/
            //profesor.idKatedre = katedra.id;
            //SaveProfesoriKatedre(profesorController);
            _storage.Save(_katedre);
            NotifyObservers();
        }

        /*public void SaveProfesoriKatedre(ProfesorController profesorController)
        {
            _profesori = profesorController;

            _serializer.ToCSV(StoragePathProfesori, profesorController.GetAllProfesors());
        }*/

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public List<Katedra> GetAll()
        {
            return _katedre;
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
