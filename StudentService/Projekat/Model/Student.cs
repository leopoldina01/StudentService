using Projekat.Manager;
using Projekat.Manager.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Projekat.Model
{
    public enum Status { B = 1, S }

    public class Student: Serializable
    {
        public int id { get; set; }
        public string prezime { get; set; }
        public string ime { get; set; }
        public DateTime datumRodjenja { get; set; }
        public Adresa adresa { get; set; }
        public int adresaId { get; set; }
        public string brTelefona { get; set; }
        public string email { get; set; }
        public string brIndeksa { get; set; }
        public uint godinaUpisa { get; set; }
        public uint godinaStudija { get; set; }
        public Status status { get; set; }
        public double prosecnaOcena { get; set; }
        public List<Ocena> polozeniPredmeti { get; set; }
        public List<Predmet> nepolozeniPredmeti { get; set; }

        public string statusStr
        {
            get
            {
                switch (status)
                {
                    case Status.B:
                        return "Budžet";
                    case Status.S:
                        return "Samofinansiranje";
                    default:
                        return "";
                }
            }
        }

        public Student()
        {
            polozeniPredmeti = new List<Ocena>();
            nepolozeniPredmeti = new List<Predmet>();
            adresaId = -1;
        }

        public Student(int id, string prezime, string ime, DateTime datumRodjenja, Adresa adresa, string brTelefona, string email, string brIndeksa, uint godinaUpisa, uint godinaStudija, Status status, double prosecnaOcena/*, List<Ocena> polozeniPredmeti, List<Predmet> nepolozeniPredmeti*/)
        {
            this.id = id;
            this.prezime = prezime;
            this.ime = ime;
            this.datumRodjenja = datumRodjenja;
            this.adresa = adresa;
            this.adresaId = adresa.Id;
            this.brTelefona = brTelefona;
            this.email = email;
            this.brIndeksa = brIndeksa;
            this.godinaUpisa = godinaUpisa;
            this.godinaStudija = godinaStudija;
            this.status = status;
            this.prosecnaOcena = prosecnaOcena;
            polozeniPredmeti = new List<Ocena>();
            nepolozeniPredmeti = new List<Predmet>();
            /*this.polozeniPredmeti = polozeniPredmeti;
            this.nepolozeniPredmeti = nepolozeniPredmeti;*/
        }

        public override string ToString()
        {
            return ime + " " + prezime;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                id.ToString(),
                prezime,
                ime,
                datumRodjenja.ToString(),
                adresaId.ToString(),
                brTelefona,
                email,
                brIndeksa,
                godinaUpisa.ToString(),
                godinaStudija.ToString(),
                status.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            id = Convert.ToInt32(values[0]);
            prezime = values[1];
            ime = values[2];
            datumRodjenja = DateTime.Parse(values[3]);
            adresaId = int.Parse(values[4]);
            brTelefona = values[5];
            email = values[6];
            brIndeksa = values[7];
            godinaUpisa = uint.Parse(values[8]);
            godinaStudija = uint.Parse(values[9]);
            switch (values[10])
            {
                case "B":
                    status = Status.B;
                    break;
                case "S":
                    status = Status.S;
                    break;
            }
        }
    }
}
