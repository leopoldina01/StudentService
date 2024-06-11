using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Manager.Serialization;
using Projekat.Model;

namespace Projekat.Manager
{
    class PredmetManager
    {
        public ParentManager parentManager { get; set; }

        private readonly string fileName = "predmeti.txt";

        public PredmetManager(ParentManager parentManager)
        {
            this.parentManager = parentManager;
            loadPredmeti();
        }

        private void loadPredmeti()
        {
            List<ManyToManySerialization> predmetiPolozili = new List<ManyToManySerialization>();
            List<ManyToManySerialization> predmetiNisuPolozili = new List<ManyToManySerialization>();

            Serializer<ManyToManySerialization> predmetiPoloziliSerializer = new Serializer<ManyToManySerialization>();
            Serializer<ManyToManySerialization> predmetiNisuPoloziliSerializer = new Serializer<ManyToManySerialization>();

            //predmeti = predmetiSerializer.FromCSV(fileName);
            predmetiPolozili = predmetiPoloziliSerializer.FromCSV("predmetiPolozili.txt");
            predmetiNisuPolozili = predmetiNisuPoloziliSerializer.FromCSV("predmetiNisuPolozili.txt");

            foreach (Predmet predmet in parentManager.predmeti)
            {
                Profesor profesor = parentManager.profesori.Find(p => p.Id == predmet.profesorId);

                if (profesor == null) continue;
                predmet.profesor = profesor;
            }

            foreach (ManyToManySerialization predmetPolozio in predmetiPolozili)
            {
                Predmet predmet = parentManager.predmeti.Find(p => p.id == predmetPolozio.id1);
                Student student = parentManager.studenti.Find(s => s.id == predmetPolozio.id2);

                if (student == null || predmet == null) continue;
                predmet.polozili.Add(student);
            }

            foreach (ManyToManySerialization predmetNijePolozio in predmetiNisuPolozili)
            {
                Predmet predmet = parentManager.predmeti.Find(p => p.id == predmetNijePolozio.id1);
                Student student = parentManager.studenti.Find(s => s.id == predmetNijePolozio.id2);

                if (predmet == null || student == null) continue;
                predmet.nisuPolozili.Add(student);
            }
        }

        private void savePredmeti()
        {
            parentManager.predmetSerializer.ToCSV(fileName, parentManager.predmeti);
            List<ManyToManySerialization> predmetiPolozili = new List<ManyToManySerialization>();
            foreach (Predmet predmet in parentManager.predmeti)
            {
                foreach (Student student in predmet.polozili)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = predmet.id;
                    temp.id2 = student.id;
                    predmetiPolozili.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> predmetiPoloziliSerializer = new Serializer<ManyToManySerialization>();
            predmetiPoloziliSerializer.ToCSV("predmetiPolozili.txt", predmetiPolozili);

            List<ManyToManySerialization> predmetiNisuPolozili = new List<ManyToManySerialization>();
            foreach (Predmet predmet in parentManager.predmeti)
            {
                foreach (Student student in predmet.nisuPolozili)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = predmet.id;
                    temp.id2 = student.id;
                    predmetiNisuPolozili.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> predmetiNisuPoloziliSerializer = new Serializer<ManyToManySerialization>();
            predmetiNisuPoloziliSerializer.ToCSV("predmetiNisuPolozili.txt", predmetiNisuPolozili);
        }

        private int generateId()
        {
            if (parentManager.predmeti.Count == 0) return 0;
            return parentManager.predmeti[parentManager.predmeti.Count - 1].id + 1;
        }

        public Predmet addPredmet(Predmet predmet)
        {
            predmet.id = generateId();
            parentManager.predmeti.Add(predmet);
            savePredmeti();
            return predmet;
        }

        public Predmet updatePredmet(Predmet predmet)
        {
            Predmet oldPredmet = GetPredmetById(predmet.id);
            if (oldPredmet == null) return null;

            oldPredmet.naziv = predmet.naziv;
            oldPredmet.semestar = predmet.semestar;
            oldPredmet.profesor = predmet.profesor;
            oldPredmet.profesorId = predmet.profesorId;
            oldPredmet.espb = predmet.espb;
            oldPredmet.polozili = predmet.polozili;
            oldPredmet.nisuPolozili = predmet.nisuPolozili;

            savePredmeti();
            return oldPredmet;
        }

        public Predmet RemovePredmet(int sifra)
        {
            Predmet predmet = GetPredmetById(sifra);
            if (predmet == null) return null;

            parentManager.predmeti.Remove(predmet);
            savePredmeti();
            return predmet;
        }

        public Predmet GetPredmetById(int sifra)
        {
            return parentManager.predmeti.Find(p => p.id == sifra);
        }

        public List<Predmet> GetAllPredmeti()
        {
            return parentManager.predmeti;
        }
    }
}

