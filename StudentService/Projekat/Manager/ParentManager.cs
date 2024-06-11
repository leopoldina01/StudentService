using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Model;
using Projekat.Manager.Serialization;

namespace Projekat.Manager
{
    class ParentManager
    {
        public List<Student> studenti { get; set; }
        public Serializer<Student> studentSerializer { get; set; }
        public List<Profesor> profesori { get; set; }
        public Serializer<Profesor> profesorSerializer { get; set; }
        public List<Predmet> predmeti { get; set; }
        public Serializer<Predmet> predmetSerializer { get; set; }
        public List<Ocena> ocene { get; set; }
        public Serializer<Ocena> ocenaSerializer { get; set; }
        public List<Katedra> katedre { get; set; }
        public Serializer<Katedra> katedraSerializer { get; set; }
        public List<Adresa> adrese { get; set; }
        public Serializer<Adresa> adresaSerializer { get; set; }

        public ParentManager()
        {
            studentSerializer = new Serializer<Student>();
            profesorSerializer = new Serializer<Profesor>();
            predmetSerializer = new Serializer<Predmet>();
            ocenaSerializer = new Serializer<Ocena>();
            katedraSerializer = new Serializer<Katedra>();
            adresaSerializer = new Serializer<Adresa>();

            Deserialize();
        }

        public void Deserialize()
        {
            studenti = studentSerializer.FromCSV("studenti.txt");
            profesori = profesorSerializer.FromCSV("profesori.txt");
            predmeti = predmetSerializer.FromCSV("predmeti.txt");
            ocene = ocenaSerializer.FromCSV("ocene.txt");
            katedre = katedraSerializer.FromCSV("katedre.txt");
            adrese = adresaSerializer.FromCSV("adrese.txt");
        }
    }
}
