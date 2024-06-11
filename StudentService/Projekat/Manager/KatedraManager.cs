using Projekat.Manager.Serialization;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Manager
{
    class KatedraManager
    {
        public ParentManager parentManager { get; set; }

        private readonly string fileName = "katedre.txt";

        public KatedraManager(ParentManager parentManager)
        {
            this.parentManager = parentManager;
            loadKatedra();
        }

        private void loadKatedra()
        {
            List<ManyToManySerialization> katedreProfesori = new List<ManyToManySerialization>();
            
            Serializer<ManyToManySerialization> katedreProfesoriSerializer = new Serializer<ManyToManySerialization>();

            //katedre = katedraSerializer.FromCSV(fileName);
            katedreProfesori = katedreProfesoriSerializer.FromCSV("katedreProfesori.txt");

            foreach (Katedra katedra in parentManager.katedre)
            {
                Profesor profesor = parentManager.profesori.Find(p => p.Id == katedra.sefId);

                if (profesor == null) continue;
                katedra.sef = profesor;
            }

            foreach (ManyToManySerialization katedraProfesor in katedreProfesori)
            {
                Katedra katedra = parentManager.katedre.Find(k => k.id == katedraProfesor.id1);
                Profesor profesor = parentManager.profesori.Find(p => p.Id == katedraProfesor.id2);

                if (katedra == null || profesor == null) continue;
                katedra.profesori.Add(profesor);
            }
        }

        private void saveKatedra()
        {
            parentManager.katedraSerializer.ToCSV(fileName, parentManager.katedre);
            List<ManyToManySerialization> katedreProfesori = new List<ManyToManySerialization>();

            foreach (Katedra katedra in parentManager.katedre)
            {
                foreach (Profesor profesor in katedra.profesori)
                {
                    ManyToManySerialization temp = new ManyToManySerialization();
                    temp.id1 = katedra.id;
                    temp.id2 = profesor.Id;
                    katedreProfesori.Add(temp);
                }
            }
            Serializer<ManyToManySerialization> katedreProfesoriSerializer = new Serializer<ManyToManySerialization>();
            katedreProfesoriSerializer.ToCSV("katedreProfesori.txt", katedreProfesori);
        }

        private int generateId()
        {
            if (parentManager.katedre.Count == 0) return 0;
            return parentManager.katedre[parentManager.katedre.Count - 1].id + 1;
        }

        public Katedra addKatedra(Katedra katedra)
        {
            katedra.id = generateId();
            parentManager.katedre.Add(katedra);
            saveKatedra();
            return katedra;
        }

        public Katedra updateKatedra(Katedra katedra)
        {
            Katedra oldKatedra = GetKatedraById(katedra.id);
            if (oldKatedra == null) return null;

            oldKatedra.naziv = katedra.naziv;
            oldKatedra.sef = katedra.sef;
            oldKatedra.sefId = katedra.sefId;
            oldKatedra.profesori = katedra.profesori;

            saveKatedra();
            return oldKatedra;
        }

        public Katedra RemoveKatedra(int sifra)
        {
            Katedra katedra = GetKatedraById(sifra);
            if (katedra == null) return null;

            parentManager.katedre.Remove(katedra);
            saveKatedra();
            return katedra;
        }

        public Katedra GetKatedraById(int sifra)
        {
            return parentManager.katedre.Find(k => k.id == sifra);
        }

        public List<Katedra> GetAllKatedre()
        {
            return parentManager.katedre;
        }
    }
}
