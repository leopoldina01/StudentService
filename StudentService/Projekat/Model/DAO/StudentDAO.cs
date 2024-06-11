using System.Collections.Generic;
using System.Linq;
using Projekat.Observer;
using Projekat.Storage;
using Projekat.Model;
using System;
using System.Globalization;
using Projekat.Controller;
using Projekat.Manager.Serialization;
using System.IO;

namespace Projekat.Model.DAO
{
    class StudentDAO : ISubject
    {
        private readonly List<IObserver> _observers;

        private readonly StudentStorage _storage;
        private readonly List<Student> _students;
        private readonly List<ManyToManySerialization> _studentiPredmeti;

        private AdresaController _adrese;
        private OcenaController _ocene;
        private PredmetController _predmeti;

        public StudentDAO()
        {
            _storage = new StudentStorage();
            _students = _storage.Load();
            _studentiPredmeti = _storage.LoadStudentiPredmeti();
            _observers = new List<IObserver>();
        }

        public void LoadComplexData(AdresaController adrese, OcenaController ocene, PredmetController predmeti)
        {
            _adrese = adrese;
            _ocene = ocene;
            _predmeti = predmeti;

            foreach (Student student in _students)
            {
                Adresa adresa = _adrese.GetAllAdrese().Find(a => a.Id == student.adresaId);
                if (adresa == null) continue;
                student.adresa = adresa;
            }

            List<ManyToManySerialization> studentiOcene = new List<ManyToManySerialization>();
            List<ManyToManySerialization> studentiPredmeti = new List<ManyToManySerialization>();

            Serializer<ManyToManySerialization> studentiOceneSerializer = new Serializer<ManyToManySerialization>();
            Serializer<ManyToManySerialization> studentiPredmetiSerializer = new Serializer<ManyToManySerialization>();

            studentiOcene = studentiOceneSerializer.FromCSV(string.Format("..{0}..{0}..{0}Data{0}studentiOcene.csv", Path.DirectorySeparatorChar));
            studentiPredmeti = studentiPredmetiSerializer.FromCSV(string.Format("..{0}..{0}..{0}Data{0}studentiPredmeti.csv", Path.DirectorySeparatorChar));

            foreach (ManyToManySerialization studentOcena in studentiOcene)
            {
                Student student = _students.Find(s => s.id == studentOcena.id1);
                Ocena ocena = _ocene.GetAllOcene().Find(o => o.Id == studentOcena.id2);
                if (ocena == null || student == null) continue;
                student.polozeniPredmeti.Add(ocena);
            }

            foreach (ManyToManySerialization studentPredmet in studentiPredmeti)
            {
                Student student = _students.Find(s => s.id == studentPredmet.id1);
                Predmet predmet = _predmeti.GetAllPredmeti().Find(p => p.id == studentPredmet.id2);
                if (student == null || predmet == null) continue;
                student.nepolozeniPredmeti.Add(predmet);
            }

            foreach (Student student in _students)
            {
                SetProsecnaOcena(student);
            }

            NotifyObservers();
        }

        public void SetProsecnaOcena(Student student)
        {
            student.prosecnaOcena = 0;
            foreach (Ocena ocena in student.polozeniPredmeti)
            {
                student.prosecnaOcena += ocena.vrednost;
            }

            if (student.polozeniPredmeti.Count != 0)
            {
                student.prosecnaOcena /= student.polozeniPredmeti.Count;
            }
        }

        public void AddNepolozeniPredmet(Student student, Predmet predmet)
        {
            student.nepolozeniPredmeti.Add(predmet);
            Save();
            NotifyObservers();
        }

        public void RemoveNepolozeniPredmet(Student student, Predmet predmet)
        {
            student.nepolozeniPredmeti.Remove(predmet);
            Save();
            NotifyObservers();
        }

        public void RemovePolozeniPredmet(Student student, Ocena ocena)
        {
            _students.Find(s => s.id == student.id).polozeniPredmeti.Remove(ocena);
            Save();
            NotifyObservers();
        }

        public uint GetEspb(int id)
        {
            Student student = _students.Find(s => s.id == id);
            uint espb = 0;
            foreach (Ocena ocena in student.polozeniPredmeti)
            {
                espb += ocena.predmet.espb;
            }
            return espb;
        }

        
        public int NextId()
        {
            if (_students.Count == 0)
                return 1;
            return _students.Max(s => s.id) + 1;
        }

        public void Add(string prezime, string ime, DateTime datumRodjenja, string adresa, string brTelefona, string email, string brIndeksa, string godinaUpisa, string godinaStudija, string status)
        {
            int id = NextId();

            int i = 0;
            int j = 0;
            Adresa adr = new Adresa();
            foreach (char c in adresa)
            {
                if (c == ',')
                    i++;
                else if (i == 0)
                {
                    if (char.IsDigit(c))
                        j++;

                    if (j == 0)
                    {
                        adr.Ulica += c;
                    }
                    else
                    {
                        adr.Broj += c;
                    }

                }
                else if (i == 1)
                {
                    adr.Grad += c;
                }
                else if (i == 2)
                {
                    adr.Drzava += c;
                }
            }
                adr.Ulica = adr.Ulica.Trim();
                adr.Broj = adr.Broj.Trim();
                adr.Grad = adr.Grad.Trim();
                adr.Drzava = adr.Drzava.Trim();

            uint godStd;
            switch(godinaStudija)
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

            Status st;
            switch (status)
            {
                case "Budžet":
                    st = Status.B;
                    break;
                case "Samofinansiranje":
                    st = Status.S;
                    break;
                default:
                    st = 0;
                    break;  
            }

            double prosecnaOcena = 0;

            foreach(Adresa adresa1 in _adrese.GetAllAdrese())
            {
                if (adresa1.Ulica != adr.Ulica) continue;
                if (adresa1.Broj != adr.Broj) continue;
                if (adresa1.Grad != adr.Grad) continue;
                if (adresa1.Drzava != adr.Drzava) continue;

                adr = adresa1;
            }
            if (adr.Id == 0)
            {
                _adrese.Create(adr.Ulica, adr.Broj, adr.Grad, adr.Drzava);
                adr = _adrese.GetAllAdrese().Find(s => s.Id == (_adrese.GetAllAdrese().Max(a => a.Id)));
            }

            Student student = new Student(id, prezime, ime, datumRodjenja, adr, brTelefona, email, brIndeksa, Convert.ToUInt32(godinaUpisa), godStd, st, prosecnaOcena);
            _students.Add(student);
            Save();
            NotifyObservers();
        }

        public void Edit(int id, string prezime, string ime, DateTime datumRodjenja, string adresa, string brTelefona, string email, string brIndeksa, string godinaUpisa, string godinaStudija, string status)
        {
            int i = 0;
            int j = 0;
            Adresa adr = new Adresa();
            foreach (char c in adresa)
            {
                if (c == ',')
                    i++;
                else if (i == 0)
                {
                    if (char.IsDigit(c))
                        j++;

                    if (j == 0)
                    {
                        adr.Ulica += c;
                    }
                    else
                    {
                        adr.Broj += c;
                    }

                }
                else if (i == 1)
                {
                    adr.Grad += c;
                }
                else if (i == 2)
                {
                    adr.Drzava += c;
                }
            }
            adr.Ulica = adr.Ulica.Trim();
            adr.Broj = adr.Broj.Trim();
            adr.Grad = adr.Grad.Trim();
            adr.Drzava = adr.Drzava.Trim();

            uint godStd;
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

            Status st;
            switch (status)
            {
                case "Budžet":
                    st = Status.B;
                    break;
                case "Samofinansiranje":
                    st = Status.S;
                    break;
                default:
                    st = 0;
                    break;
            }

            double prosecnaOcena = 0;

            foreach (Adresa adresa1 in _adrese.GetAllAdrese())
            {
                if (adresa1.Ulica != adr.Ulica) continue;
                if (adresa1.Broj != adr.Broj) continue;
                if (adresa1.Grad != adr.Grad) continue;
                if (adresa1.Drzava != adr.Drzava) continue;

                adr = adresa1;
            }
            if (adr.Id == 0)
            {
                _adrese.Create(adr.Ulica, adr.Broj, adr.Grad, adr.Drzava);
                adr = _adrese.GetAllAdrese().Find(s => s.Id == (_adrese.GetAllAdrese().Max(a => a.Id)));
            }


            Student student = getStudentById(id);
            student.prezime = prezime;
            student.ime = ime;
            student.datumRodjenja= datumRodjenja;
            student.adresa = adr;
            student.adresaId = adr.Id;
            student.brTelefona = brTelefona;
            student.email = email;
            student.brIndeksa = brIndeksa;
            student.godinaUpisa = Convert.ToUInt32(godinaUpisa);
            student.godinaStudija = godStd;
            student.status = st;
            student.prosecnaOcena = prosecnaOcena;

            Save();
            NotifyObservers();
        }

        public void Remove(Student student)
        {
            _students.Remove(student);
            Save();
            NotifyObservers();
        }

        public void Save()
        {
            List<ManyToManySerialization> studentiOcene = new List<ManyToManySerialization>();
            foreach (Student student in _students)
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
            foreach (Student student in _students)
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

            _storage.Save(_students);
        }

        public Student getStudentById(int id)
        {
            return _students.Find(s => s.id == id);
        }

        public List<Student> GetAll()
        {
            return _students;
        }

        /*public List<Ocena> GetAllOcene(int id)
        {
            Student student = _students.Find(s => s.id == id);
            return student.polozeniPredmeti;
        }*/

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
