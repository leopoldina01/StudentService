using Projekat.Controller;
using Projekat.Manager.Serialization;
using Projekat.Observer;
using Projekat.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Projekat.Model.DAO
{
    public class PredmetDAO : ISubject
    {
        private ProfesorController _profesori;
        private StudentController _studenti;

        private readonly List<IObserver> _observers;

        private readonly PredmetStorage _storage;
        private readonly List<Predmet> _predmeti;
        private List<Student> _students;

        public PredmetDAO()
        {
            _storage = new PredmetStorage();
            _predmeti = _storage.Load();
            _observers = new List<IObserver>();
        }

        public void DodajProfesora(Predmet predmet, Profesor profesor, ProfesorController profesorController)
        {
            predmet.imePrezimeProfesora = profesor.ImePrezime;
            predmet.profesor = profesor;
            predmet.profesorId = profesor.Id;
            _profesori = profesorController;
            _profesori.GetAllProfesors().Find(p => p.Id == profesor.Id).predmeti.Add(predmet);
            SaveProfesoriPredmeti(profesorController);
            Save();
            NotifyObservers();
        }

        public void SaveProfesoriPredmeti(ProfesorController profesorController)
        {
            _profesori = profesorController; 

            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();

            foreach (Profesor profesor in _profesori.GetAllProfesors())
            {

                foreach (Predmet predmet in profesor.predmeti)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = profesor.Id;
                    temp.id2 = predmet.id;
                    if (profesorPredmeti.Find(i => i.id1 == temp.id1 && i.id2 == temp.id2) == null)
                    {
                        profesorPredmeti.Add(temp);
                    }
                }
            }

            Serializer<ManyToManySerialization> profesorPredmetiSerializer = new Serializer<ManyToManySerialization>();
            profesorPredmetiSerializer.ToCSV(string.Format("..{0}..{0}..{0}Data{0}profesorPredmeti.csv", Path.DirectorySeparatorChar), profesorPredmeti);


        }

        public void ObrisiProfesora(Predmet predmet, Profesor profesor, ProfesorController profesorController)
        {
            predmet.imePrezimeProfesora = null;
            predmet.profesor = null;
            predmet.profesorId = -1;
            _profesori = profesorController;
            if (_profesori.GetAllProfesors().Find(p => p.Id == profesor.Id) != null)
            {
                _profesori.GetAllProfesors().Find(p => p.Id == profesor.Id).predmeti.Remove(predmet);
            }
            SaveProfesoriPredmeti(profesorController);
            Save();
            NotifyObservers();
        }

        public void VratiProfesora(Profesor profesor, Predmet predmet, Profesor SelectedPredmetProfesor, ProfesorController _profesorController)
        {
            if (profesor == null)
            {
                ObrisiProfesora(predmet, SelectedPredmetProfesor, _profesorController);

            } else if (SelectedPredmetProfesor == null)
            {
                DodajProfesora(predmet, profesor, _profesorController);
            } else
            {
                ObrisiProfesora(predmet, SelectedPredmetProfesor, _profesorController);
                DodajProfesora(predmet, profesor, _profesorController);
            }
        }

        public void LoadComplexData(ProfesorController profesori)
        {
            _profesori = profesori;

            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();

            Serializer<ManyToManySerialization> profesorPredmetiSerializer = new Serializer<ManyToManySerialization>();

            profesorPredmeti = profesorPredmetiSerializer.FromCSV(string.Format("..{0}..{0}..{0}Data{0}profesorPredmeti.csv", Path.DirectorySeparatorChar));

            foreach (ManyToManySerialization profesorPredmet in profesorPredmeti)
            {
                Predmet predmet = _predmeti.Find(p => p.id == profesorPredmet.id2);
                Profesor profesor = _profesori.GetAllProfesors().Find(p => p.Id == profesorPredmet.id1);
                if (predmet == null || profesor == null) continue;
                predmet.profesor = profesor;
                //profesor.predmeti.Add(predmet);
            }

            NotifyObservers();
        }

        public List<Predmet> GetAllPredmeti()
        {
            return _predmeti;
        }

        public int NextId()
        {
            if (_predmeti.Count == 0)
                return 1;
            return _predmeti.Max(s => s.id) + 1;
        }

        public void Add(string sifra, string naziv, string semestar, string godinaStudija, string espb)
        {
            int id = NextId();

            Semestar sem;
            switch (semestar)
            {
                case "Letnji":
                    sem = Semestar.Letnji;
                    break;
                case "Zimski":
                    sem = Semestar.Zimski;
                    break;
                default:
                    sem = 0;
                    break;
            }

            int godStd;
            switch (godinaStudija)
            {
                case "Prva":
                    godStd = 1;
                    break;
                case "Druga":
                    godStd = 2;
                    break;
                case "Treća":
                    godStd = 3;
                    break;
                case "Četvrta":
                    godStd = 4;
                    break;
                default:
                    godStd = 0;
                    break;
            }

            Predmet predmet = new Predmet(id, sifra, naziv, sem, godStd, Convert.ToUInt32(espb));
            _predmeti.Add(predmet);
            _storage.Save(_predmeti);
            NotifyObservers();
        }

        public void Edit(int id, string sifra, string naziv, string semestar, string godinaStudija, string espb)
        {
            Semestar sem;
            switch (semestar)
            {
                case "Letnji":
                    sem = Semestar.Letnji;
                    break;
                case "Zimski":
                    sem = Semestar.Zimski;
                    break;
                default:
                    sem = 0;
                    break;
            }

            int godStd;
            switch (godinaStudija)
            {
                case "Prva":
                    godStd = 1;
                    break;
                case "Druga":
                    godStd = 2;
                    break;
                case "Treća":
                    godStd = 3;
                    break;
                case "Četvrta":
                    godStd = 4;
                    break;
                default:
                    godStd = 0;
                    break;
            }

            Predmet predmet = getPredmetById(id);
            predmet.sifra = sifra;
            predmet.naziv = naziv;
            predmet.semestar = sem;
            predmet.godinaStudija = godStd;
            predmet.espb = Convert.ToUInt32(espb);

            _storage.Save(_predmeti);
            NotifyObservers();
        }

        public void Remove(Predmet predmet, StudentController studentController, OcenaController ocenaController, ProfesorController profesorController)
        {
            _predmeti.Remove(predmet);
            //_storage.Save(_predmeti);
            Save();
            SaveStudentiPredmeti(studentController, predmet, ocenaController);
            SaveProfesoriObrisaniPredmet(predmet, profesorController);
            NotifyObservers();
        }

        public void SaveProfesoriObrisaniPredmet(Predmet predmet, ProfesorController profesorController)
        {
            foreach (Profesor profesor in profesorController.GetAllProfesors())
            {
                profesorController.ObrisiPredmet(profesor, predmet);
            }
        }

        public void SaveStudentiPredmeti(StudentController studentController, Predmet predmet, OcenaController ocenaController)
        {
            _students = studentController.GetAllStudents();

            foreach (Student student in _students)
            {
                studentController.RemoveNepolozeniPredmet(student, predmet);

                foreach (Ocena ocena in ocenaController.GetAllOcene())
                {
                    if (ocena.predmetId == predmet.id)
                    {
                        studentController.RemovePolozeniPredmet(student, ocena);
                    }
                }
            }

            _storage.Save(_predmeti);
            NotifyObservers();
            //_storage.Save(students);
        }

        public void Save()
        {
            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();
            foreach (Predmet predmet in _predmeti)
            {
                    
                ManyToManySerialization temp = new ManyToManySerialization();
                if (predmet.profesor != null)
                {
                    temp.id1 = predmet.profesorId;
                    temp.id2 = predmet.id;
                    if (profesorPredmeti.Find(i => i.id1 == temp.id1 && i.id2 == temp.id2) == null && predmet.profesorId != -1)
                    {
                        profesorPredmeti.Add(temp);
                    }
                }
            }

            Serializer<ManyToManySerialization> profesorPredmetiSerializer = new Serializer<ManyToManySerialization>();
            profesorPredmetiSerializer.ToCSV(string.Format("..{0}..{0}..{0}Data{0}profesorPredmeti.csv", Path.DirectorySeparatorChar), profesorPredmeti);

            _storage.Save(_predmeti);
        }

        public Predmet getPredmetById(int sifra)
        {
            return _predmeti.Find(p => p.id == sifra);
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
