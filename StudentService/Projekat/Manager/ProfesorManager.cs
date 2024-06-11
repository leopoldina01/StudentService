using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Manager.Serialization;
using Projekat.Model;

namespace Projekat.Manager
{
    class ProfesorManager
    {
        public ParentManager parentManager { get; set; }

        private readonly string fileName = "profesori.txt";

        public ProfesorManager(ParentManager parentManager)
        {
            this.parentManager = parentManager;
            LoadProfesor();
        }

        private void LoadProfesor()
        {
            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();
            Serializer<ManyToManySerialization> profesorPredmetiSerializer = new Serializer<ManyToManySerialization>();
            profesorPredmeti = profesorPredmetiSerializer.FromCSV("profesorPredmeti.txt");

            foreach (Profesor profesor in parentManager.profesori)
            {
                Adresa adresaK = parentManager.adrese.Find(a => a.Id == profesor.adresaKancId);
                if (adresaK != null)
                {
                    profesor.adresaKanc = adresaK;
                }

                Adresa adresaS = parentManager.adrese.Find(a => a.Id == profesor.adresaStanId);
                if (adresaS != null)
                {
                    profesor.adresaStan = adresaS;
                }
            }

            foreach (ManyToManySerialization profesorPredmet in profesorPredmeti)
            {
                Profesor profesor = parentManager.profesori.Find(p => p.Id == profesorPredmet.id1);
                Predmet predmet = parentManager.predmeti.Find(p => p.id == profesorPredmet.id2);
                if (profesor != null && predmet != null)
                {
                    profesor.predmeti.Add(predmet);
                }
            }
            
        }

        private void SaveProfesor()
        {
            parentManager.profesorSerializer.ToCSV(fileName, parentManager.profesori);

            List<ManyToManySerialization> profesorPredmeti = new List<ManyToManySerialization>();
            foreach (Profesor profesor in parentManager.profesori)
            {
                foreach (Predmet predmet in profesor.predmeti)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = profesor.Id;
                    temp.id2 = predmet.id;
                    profesorPredmeti.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> profesorPredmetiSerializer = new Serializer<ManyToManySerialization>();
            profesorPredmetiSerializer.ToCSV("profesorPredmeti.txt", profesorPredmeti);
            
        }

        private int generateId()
        {
            if (parentManager.profesori.Count == 0)
            {
                return 0;
            }
            else
            {
                return parentManager.profesori[parentManager.profesori.Count - 1].Id + 1;
            }
        }

        public Profesor AddProfesor(Profesor pr)
        {
            pr.Id = generateId();
            parentManager.profesori.Add(pr);
            SaveProfesor();
            return pr;
        }

        public Profesor GetProfesorById(int id)
        {
            return parentManager.profesori.Find(p => p.Id == id);
        }

        public Profesor UpdateProfesor(Profesor pr)
        { 
            Profesor oldProfesor = GetProfesorById(pr.Id);
            if (oldProfesor == null)
            {
                return null;
            }

            oldProfesor.Id = pr.Id;
            oldProfesor.prezime = pr.prezime;
            oldProfesor.ime = pr.ime;
            oldProfesor.datumRodj = pr.datumRodj;
            oldProfesor.adresaStan = new Adresa(pr.adresaStan);
            oldProfesor.adresaStanId = pr.adresaStan.Id;
            oldProfesor.kontakt = pr.kontakt;
            oldProfesor.email = pr.email;
            oldProfesor.adresaKanc = new Adresa(pr.adresaKanc);
            oldProfesor.adresaKancId = pr.adresaKanc.Id;
            oldProfesor.brlk = pr.brlk;
            oldProfesor.zvanje = pr.zvanje;
            oldProfesor.godineStaza = pr.godineStaza;
            oldProfesor.predmeti = pr.predmeti;

            SaveProfesor();
            return oldProfesor;
        }

        public Profesor RemoveProfesor(int id)
        {
            Profesor profesor = GetProfesorById(id);

            if (profesor == null)
            {
                return null;
            }

            parentManager.profesori.Remove(profesor);
            SaveProfesor();
            return profesor;
        }

        public List<Profesor> GetAllProfessors()
        {
            return parentManager.profesori;
        }
    }
}
