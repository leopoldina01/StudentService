using Projekat.Controller;
using Projekat.Manager;
using Projekat.Manager.Serialization;
using Projekat.Observer;
using Projekat.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows;

namespace Projekat.Model.DAO
{
    class ProfesorDAO : ISubject
    {
        private readonly List<IObserver> _observers;

        private readonly ProfesorStorage _storage;
        private readonly List<Profesor> _profesori;

        private AdresaController _adresaController;

        public ProfesorDAO()
        {
            _storage = new ProfesorStorage();
            _profesori = _storage.Load();
            _observers = new List<IObserver>();
        }

        public void LoadComplexData(AdresaController adrese, PredmetController predmeti)
        {
            _adresaController = adrese;

            foreach (Profesor profesor in _profesori)
            {
                Adresa adresa2 = _adresaController.GetAllAdrese().Find(a => a.Id == profesor.adresaStanId);
                if (adresa2 != null)
                {
                    profesor.adresaStan = adresa2;
                }

                Adresa adresa1 = _adresaController.GetAllAdrese().Find(a => a.Id == profesor.adresaKancId);
                if (adresa1 != null)
                {
                    profesor.adresaKanc = adresa1;
                }
            }

            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();
            Serializer<ManyToManySerialization> profesorPredmetiSerializer = new Serializer<ManyToManySerialization>();
            profesorPredmeti = profesorPredmetiSerializer.FromCSV(string.Format("..{0}..{0}..{0}Data{0}profesorPredmeti.csv", Path.DirectorySeparatorChar));

            foreach (ManyToManySerialization profesorPredmet in profesorPredmeti)
            {
                Profesor profesor = _profesori.Find(p => p.Id == profesorPredmet.id1);
                Predmet predmet = predmeti.GetAllPredmeti().Find(p => p.id == profesorPredmet.id2);
                if (profesor != null && predmet != null)
                {
                    profesor.predmeti.Add(predmet);
                }
            }

            NotifyObservers();
        }

        public void DodajPredmet(Profesor profesor, Predmet predmet)
        {
            profesor.predmeti.Add(predmet);
            predmet.profesor = profesor;
            Save();
            NotifyObservers();
        }

        public void ObrisiPredmet(Profesor profesor, Predmet predmet)
        {
            profesor.predmeti.Remove(predmet);
            predmet.profesor = null;
            Save();
            NotifyObservers();
        }

        private int generateId()
        {
            if (_profesori.Count == 0)
            {
                return 0;
            }
            else
            {
                return _profesori[_profesori.Count - 1].Id + 1;
            }
        }

        //ili ovako ili preko profesora da se dodaje a ne da se
        //koristi konstruktor sa parametrima vec konstruktor kopije
        public void AddProfesor(string prezime, string ime, DateTime datumRodj, string adresaStan, string kontakt, string email, string adresaKanc, string brlk, string zvanje, string godineStaza)
        {
            int id = generateId();
            int i = 0;
            int j = 0;
            Adresa adr = new Adresa();
            foreach (char c in adresaStan)
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

            foreach (Adresa adresa1 in _adresaController.GetAllAdrese())
            {
                if (adresa1.Ulica != adr.Ulica) continue;
                if (adresa1.Broj != adr.Broj) continue;
                if (adresa1.Grad != adr.Grad) continue;
                if (adresa1.Drzava != adr.Drzava) continue;

                adr = adresa1;
            }

            if (adr.Id == 0)
            {
                _adresaController.Create(adr.Ulica, adr.Broj, adr.Grad, adr.Drzava);
                adr = _adresaController.GetAllAdrese().Find(s => s.Id == (_adresaController.GetAllAdrese().Max(a => a.Id)));
            }

            i = 0;
            j = 0;
            Adresa adrKanc = new Adresa();
            foreach (char c in adresaKanc)
            {
                if (c == ',')
                    i++;
                else if (i == 0)
                {
                    if (char.IsDigit(c))
                        j++;

                    if (j == 0)
                    {
                        adrKanc.Ulica += c;
                    }
                    else
                    {
                        adrKanc.Broj += c;
                    }

                }
                else if (i == 1)
                {
                    adrKanc.Grad += c;
                }
                else if (i == 2)
                {
                    adrKanc.Drzava += c;
                }
            }
            adrKanc.Ulica = adrKanc.Ulica.Trim();
            adrKanc.Broj = adrKanc.Broj.Trim();
            adrKanc.Grad = adrKanc.Grad.Trim();
            adrKanc.Drzava = adrKanc.Drzava.Trim();

            foreach (Adresa adresa1 in _adresaController.GetAllAdrese())
            {
                if (adresa1.Ulica != adrKanc.Ulica) continue;
                if (adresa1.Broj != adrKanc.Broj) continue;
                if (adresa1.Grad != adrKanc.Grad) continue;
                if (adresa1.Drzava != adrKanc.Drzava) continue;

                adrKanc = adresa1;
            }

            if (adrKanc.Id == 0)
            {
                _adresaController.Create(adrKanc.Ulica, adrKanc.Broj, adrKanc.Grad, adrKanc.Drzava);
                adrKanc = _adresaController.GetAllAdrese().Find(s => s.Id == (_adresaController.GetAllAdrese().Max(a => a.Id)));
            }

            Profesor profesor = new Profesor(id, prezime, ime, datumRodj, adr, kontakt, email, adrKanc, brlk, zvanje, Convert.ToInt32(godineStaza));
            _profesori.Add(profesor);
            _storage.Save(_profesori);
            NotifyObservers();
        }

        public void Edit(int id, string prezime, string ime, DateTime datumRodj, string adresaStan, string kontakt, string email, string adresaKanc, string brlk, string zvanje, string godineStaza)
        {
            int i = 0;
            int j = 0;
            Adresa adrStan = new Adresa();
            foreach (char c in adresaStan)
            {
                if (c == ',')
                    i++;
                else if (i == 0)
                {
                    if (char.IsDigit(c))
                        j++;

                    if (j == 0)
                    {
                        adrStan.Ulica += c;
                    }
                    else
                    {
                        adrStan.Broj += c;
                    }

                }
                else if (i == 1)
                {
                    adrStan.Grad += c;
                }
                else if (i == 2)
                {
                    adrStan.Drzava += c;
                }
            }

            adrStan.Ulica = adrStan.Ulica.Trim();
            adrStan.Broj = adrStan.Broj.Trim();
            adrStan.Grad = adrStan.Grad.Trim();
            adrStan.Drzava = adrStan.Drzava.Trim();

            foreach (Adresa adresa1 in _adresaController.GetAllAdrese())
            {
                if (adresa1.Ulica != adrStan.Ulica) continue;
                if (adresa1.Broj != adrStan.Broj) continue;
                if (adresa1.Grad != adrStan.Grad) continue;
                if (adresa1.Drzava != adrStan.Drzava) continue;

                adrStan = adresa1;
            }

            if (adrStan.Id == 0)
            {
                _adresaController.Create(adrStan.Ulica, adrStan.Broj, adrStan.Grad, adrStan.Drzava);
                adrStan = _adresaController.GetAllAdrese().Find(s => s.Id == (_adresaController.GetAllAdrese().Max(a => a.Id)));
            }

            i = 0;
            j = 0;
            Adresa adrKanc = new Adresa();
            foreach (char c in adresaKanc)
            {
                if (c == ',')
                    i++;
                else if (i == 0)
                {
                    if (char.IsDigit(c))
                        j++;

                    if (j == 0)
                    {
                        adrKanc.Ulica += c;
                    }
                    else
                    {
                        adrKanc.Broj += c;
                    }

                }
                else if (i == 1)
                {
                    adrKanc.Grad += c;
                }
                else if (i == 2)
                {
                    adrKanc.Drzava += c;
                }
            }

            adrKanc.Ulica = adrKanc.Ulica.Trim();
            adrKanc.Broj = adrKanc.Broj.Trim();
            adrKanc.Grad = adrKanc.Grad.Trim();
            adrKanc.Drzava = adrKanc.Drzava.Trim();

            foreach (Adresa adresa1 in _adresaController.GetAllAdrese())
            {
                if (adresa1.Ulica != adrKanc.Ulica) continue;
                if (adresa1.Broj != adrKanc.Broj) continue;
                if (adresa1.Grad != adrKanc.Grad) continue;
                if (adresa1.Drzava != adrKanc.Drzava) continue;

                adrKanc = adresa1;
            }

            if (adrKanc.Id == 0)
            {
                _adresaController.Create(adrKanc.Ulica, adrKanc.Broj, adrKanc.Grad, adrKanc.Drzava);
                adrKanc = _adresaController.GetAllAdrese().Find(s => s.Id == (_adresaController.GetAllAdrese().Max(a => a.Id)));
            }

            Profesor profesor = getProfesorById(id);
            profesor.prezime = prezime;
            profesor.ime = ime;
            profesor.datumRodj = datumRodj;
            profesor.adresaStan = adrStan;
            profesor.adresaStanId = adrStan.Id;
            profesor.kontakt = kontakt;
            profesor.email = email;
            profesor.adresaKanc = adrKanc;
            profesor.adresaKancId = adrKanc.Id;
            profesor.brlk = brlk;
            profesor.zvanje = zvanje;
            profesor.godineStaza = Convert.ToInt32(godineStaza);
            

            _storage.Save(_profesori);
            NotifyObservers();
        }

        public Profesor getProfesorById(int id)
        {
            return _profesori.Find(s => s.Id == id);
        }

        public List<Profesor> GetAllProfessors()
        {
            return _profesori;
        }

        public void Remove(Profesor profesor, PredmetController predmetController, ProfesorController profesorController)
        {
            _profesori.Remove(profesor);
            _storage.Save(_profesori);
            SaveUklonjenProfesorPredmet(profesor, predmetController, profesorController);
            NotifyObservers();
        }

        public void SaveUklonjenProfesorPredmet(Profesor profesor, PredmetController predmetController, ProfesorController profesorController)
        {
            foreach (Predmet predmet in predmetController.GetAllPredmeti())
            {
                if (predmet.profesorId == profesor.Id)
                {
                    predmetController.ObrisiProfesora(predmet, profesor, profesorController);
                    //MessageBox.Show("obrisan profesor sa predmeta");
                }
            }
        }

        public void Save()
        {
            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();
            foreach (Profesor profesor in _profesori)
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


            _storage.Save(_profesori);
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
