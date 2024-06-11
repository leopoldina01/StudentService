using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Manager.Serialization;
using Projekat.Model;

namespace Projekat.Manager
{
    class OcenaManager
    {
        public ParentManager parentManager { get; set; }

        private readonly string fileName = "ocene.txt";

        public OcenaManager(ParentManager parentManager)
        {
            this.parentManager = parentManager;
            loadOcene();
        }

        private void loadOcene()
        {
            foreach (Ocena ocena in parentManager.ocene)
            {
                Student polozio = parentManager.studenti.Find(p => p.id == ocena.polozioId);
                if (polozio != null)
                {
                    ocena.polozio = polozio;
                }
                else
                {
                    continue;
                }

                Predmet predmet = parentManager.predmeti.Find(p => p.id == ocena.predmetId);
                if (predmet != null)
                {
                    ocena.predmet = predmet;
                }
                else
                {
                    continue;
                }
            }
        }

        public void saveOcene()
        {
            parentManager.ocenaSerializer.ToCSV(fileName, parentManager.ocene);
            List<ManyToManySerialization> studentiOcene = new List<ManyToManySerialization>();
            foreach (Ocena ocena in parentManager.ocene)
            {
                Student polozio = parentManager.studenti.Find(p => p.id == ocena.polozioId);
                if (polozio != null)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = polozio.id;
                    temp.id2 = ocena.Id;
                    studentiOcene.Add(temp);
                    polozio.polozeniPredmeti.Add(ocena);
                    polozio.polozeniPredmeti = polozio.polozeniPredmeti.Distinct().ToList();
                }
                else
                {
                    continue;
                }
            }
            Serializer<ManyToManySerialization> studentiOceneSerializer = new Serializer<ManyToManySerialization>();
            studentiOceneSerializer.ToCSV("studentiOcene.txt", studentiOcene);
        }

        private int generateId()
        {
            if (parentManager.ocene.Count == 0)
            {
                return 0;
            }
            return parentManager.ocene[parentManager.ocene.Count - 1].Id + 1;
        }

        public Ocena addOcena(Ocena ocena)
        {
            ocena.Id = generateId();
            parentManager.ocene.Add(ocena);
            saveOcene();
            return ocena;
        }

        public Ocena getOcenaById(int id)
        {
            return parentManager.ocene.Find(o => o.Id == id);
        }

        public Ocena updateOcena(Ocena ocena)
        {
            Ocena oldOcena = getOcenaById(ocena.Id);
            if (oldOcena == null)
            {
                return null;
            }

            oldOcena.polozio = ocena.polozio;
            oldOcena.polozioId = ocena.polozioId;
            oldOcena.predmet = ocena.predmet;
            oldOcena.vrednost = ocena.vrednost;
            oldOcena.datumPolaganja = ocena.datumPolaganja;

            saveOcene();
            return oldOcena;
        }

        public Ocena RemoveOcena(int id)
        {
            Ocena ocena = getOcenaById(id);
            if (ocena == null)
            {
                return null;
            }

            parentManager.ocene.Remove(ocena);
            saveOcene();
            return ocena;
        }

        public List<Ocena> GetAllOcene()
        {
            return parentManager.ocene;
        }
    }
}
