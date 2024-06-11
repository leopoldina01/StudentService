using Projekat.Manager.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Model
{
    public class Ocena : Serializable
    {
        public int Id { get; set; }
        public Student polozio { get; set; }
        public int polozioId { get; set; }
        public Predmet predmet { get; set; }
        public int predmetId { get; set; }
        public double vrednost { get; set; }
        public DateTime datumPolaganja { get; set; } 

        public Ocena() 
        {
            polozioId = -1;
            predmetId = -1;
        }

        public Ocena(int id, Student Polozio, Predmet Predmet, double Vrednost, DateTime DatumPolaganja)
        {
            Id = id;
            polozio = Polozio;
            polozioId = polozio.id;
            predmet = Predmet;
            predmetId = predmet.id;
            vrednost = Vrednost;
            datumPolaganja = DatumPolaganja;
        }

        public override string ToString()
        {
            string ocenaStr = string.Format("\nID: {0}", Id);
            if (polozio != null)
            {
                ocenaStr += "\n STUDENT POLOZIO: ";
                ocenaStr += polozio.brIndeksa;
            }
            else
            {
                ocenaStr += "\n STUDENT POLOZIO: ";
            }
            if (predmet != null)
            {
                ocenaStr += "\n PREDMET: ";
                ocenaStr += predmet.naziv;
            }
            else
            {
                ocenaStr += "\n PREDMET: ";
            }
            ocenaStr += "\n VREDNOST: ";
            ocenaStr += vrednost.ToString();

            ocenaStr += string.Format("\n DATUM POLAGANJA: {0:d}\n", datumPolaganja);
            return ocenaStr;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                polozioId.ToString(),
                predmetId.ToString(),
                vrednost.ToString(),
                datumPolaganja.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            polozioId = int.Parse(values[1]);
            predmetId = int.Parse(values[2]);
            vrednost = double.Parse(values[3]);
            datumPolaganja = DateTime.Parse(values[4]);
        }
    }
}
