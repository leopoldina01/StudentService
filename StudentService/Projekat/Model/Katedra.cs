using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Manager.Serialization;

namespace Projekat.Model
{
    public class Katedra : Serializable
    {
        public int id { get; set; }
        public string sifra { get; set; }
        public string naziv { get; set; }
        public Profesor sef { get; set; }
        public int sefId { get; set; }
        public List<Profesor> profesori { get; set; }

        public Katedra()
        {
            profesori = new List<Profesor>();
            sefId = -1;
        }

        public Katedra(int id, string sifra, string naziv, int sefId, List<Profesor> profesori)
        {
            this.id = id;
            this.sifra = sifra;
            this.naziv = naziv;
            this.sefId = sefId;
            this.profesori = profesori;
        }

        public override string ToString()
        {
            string katedraStr = String.Format("Sifra: {0} \n Naziv: {1} \n ", id, naziv);

            katedraStr += "Sef: ";
            if (sef != null)
                katedraStr += sef.ime + " " + sef.prezime;
            katedraStr += " \n";

            katedraStr += " Profesori: ";
            foreach (Profesor profesor in profesori)
            {
                if (profesor == null) continue;
                katedraStr += profesor.ime + " " + profesor.prezime;
                katedraStr += ", ";
            }
            katedraStr = katedraStr.TrimEnd(',', ' ');
            katedraStr += " \n";

            return katedraStr;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                id.ToString(),
                sifra,
                naziv,
                sefId.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            sifra = values[1];
            naziv = values[2];
            sefId = int.Parse(values[3]);
        }
    }
}
