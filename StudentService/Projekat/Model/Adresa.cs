using Projekat.Manager.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Model
{
    public class Adresa : Serializable
    {
        public int Id { get; set; }
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }

        public Adresa() { }

        public Adresa(int id, string ulica, string broj, string grad, string drzava)
        {
            Id = id;
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public Adresa(Adresa a)
        {
            Id = a.Id;
            Ulica = a.Ulica;
            Broj = a.Broj;
            Grad = a.Grad;
            Drzava = a.Drzava;
        }

        public override string ToString()
        {
            return Ulica + " " + Broj + ", " + Grad + ", " + Drzava;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Ulica,
                Broj.ToString(),
                Grad,
                Drzava
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Ulica = values[1];
            Broj = values[2];
            Grad = values[3];
            Drzava = values[4];
        }
    }
}
