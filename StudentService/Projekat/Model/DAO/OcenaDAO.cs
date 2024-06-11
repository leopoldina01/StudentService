using Projekat.Controller;
using Projekat.Manager;
using Projekat.Manager.Serialization;
using Projekat.Observer;
using Projekat.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projekat.Model.DAO
{
    public class OcenaDAO : ISubject
    {
        private readonly List<IObserver> _observers;

        private readonly OcenaStorage _storage;
        private readonly StudentStorage _studentStorage;
        private readonly PredmetStorage _predmetStorage;
        private readonly List<Ocena> _ocene;
        private readonly List<Student> studenti;
        private readonly List<Predmet> predmeti;

        private StudentController _studenti;
        private PredmetController _predmeti;
        private readonly StudentController _studentController;
        private readonly PredmetController _predmetController;

        public OcenaDAO() 
        {
            _storage = new OcenaStorage();
            _ocene = _storage.Load();
            _studentStorage = new StudentStorage();
            studenti = _studentStorage.Load();
            _predmetStorage = new PredmetStorage();
            predmeti = _predmetStorage.Load();

            _observers = new List<IObserver>();
        }

        public void LoadComplexData(StudentController studenti, PredmetController predmeti)
        {
            _studenti = studenti;
            _predmeti = predmeti;

            foreach (Ocena ocena in _ocene)
            {
                Student student = _studenti.GetAllStudents().Find(s => s.id == ocena.polozioId);
                if (student == null) continue;
                    ocena.polozio = student;

                Predmet predmet = _predmeti.GetAllPredmeti().Find(p => p.id == ocena.predmetId);
                if (predmet == null) continue;
                ocena.predmet = predmet;
            }
        }

        int generateId()
        {
            if (_ocene.Count == 0)
            {
                return 0;
            }
            else
            {
                return _ocene[_ocene.Count - 1].Id + 1;
            }
        }

        public void Add(Student student, Predmet predmet, double vrednost, DateTime datumPolaganje)
        {
            int id = generateId();
            Ocena ocena = new Ocena(id, student, predmet, vrednost, datumPolaganje);
            _ocene.Add(ocena);

            Save();
            NotifyObservers();
        }

        public void Edit()
        {
            throw new NotImplementedException();
            }

        public void Remove(Ocena ocena)
        {
            _ocene.Remove(ocena);
            Save();
            NotifyObservers();
        }

        public void Save() 
        {
            _storage.Save(_ocene);
        }

        public void SaveComplexData(StudentController s, PredmetController p)
        {
            _studenti = s;
            _predmeti = p;

            List<ManyToManySerialization> studentiOcene = new List<ManyToManySerialization>();
            foreach (Student student in _studenti.GetAllStudents())
            {
                foreach (Ocena ocena in student.polozeniPredmeti)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = student.id;
                    temp.id2 = ocena.Id;
                    studentiOcene.Add(temp);
                }
            }

            Serializer<ManyToManySerialization> studentiOceneSerializer = new Serializer<ManyToManySerialization>();
            studentiOceneSerializer.ToCSV(string.Format("..{0}..{0}..{0}Data{0}studentiOcene.csv", Path.DirectorySeparatorChar), studentiOcene);


            List<ManyToManySerialization> studentiPredmeti = new List<ManyToManySerialization>();
            foreach (Student student in _studenti.GetAllStudents())
            {
                foreach (Predmet predmet in student.nepolozeniPredmeti)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = student.id;
                    temp.id2 = predmet.id;
                    studentiPredmeti.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> studentiPredmetiSerializer = new Serializer<ManyToManySerialization>();
            studentiPredmetiSerializer.ToCSV(string.Format("..{0}..{0}..{0}Data{0}studentiPredmeti.csv", Path.DirectorySeparatorChar), studentiPredmeti);
            NotifyObservers();
        }

        public Ocena getStudentById(int id)
        {
            return _ocene.Find(o => o.Id == id);
        }

        public List<Ocena> GetAll()
        {
            return _ocene;
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
