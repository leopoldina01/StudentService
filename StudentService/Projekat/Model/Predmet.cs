//mozda ce svugde gde je predmet trebati da se doda i unos godine studija
//ali to trenutno za prikaz predmeta nije potrebno
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using Projekat.Manager.Serialization;

namespace Projekat.Model
{
    public enum Semestar
    {
        Letnji = 1,
        Zimski
    }

    public class Predmet : Serializable
    {
        public int id { get; set; }
        public string sifra { get; set; }
        public string naziv {  get; set; }
        public Semestar semestar { get; set; }
        public int godinaStudija { get; set; }
        public Profesor profesor { get; set; }
        public int profesorId { get; set; }

        public uint espb { get; set; }
        public List<Student> polozili { get; set; }
        public List<Student> nisuPolozili { get; set; }

        public string imePrezimeProfesora { get; set; }

        public string SifraNaziv
        {
            get
            {
                return sifra + " - " + naziv;
            }
        }

        public Predmet()
        {
            polozili = new List<Student>();
            nisuPolozili = new List<Student>();
            profesorId = -1;
        }

        public Predmet(int id, string sifra, string naziv, Semestar semestar, int godinaStudija, uint espb)
        {
            this.id = id;
            this.sifra = sifra;
            this.naziv = naziv;
            this.semestar = semestar;
            this.godinaStudija = godinaStudija;
            //this.profesor = profesor;
            this.espb = espb;
            //this.profesor = new Profesor();
            this.polozili = new List<Student>();
            this.nisuPolozili = new List<Student>();
            /*this.polozili = polozili;
            this.nisuPolozili = nisuPolozili;*/
        }

        public override string ToString()
        {
            return SifraNaziv;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                id.ToString(),
                sifra,
                naziv,
                semestar.ToString(),
                godinaStudija.ToString(),
                profesorId.ToString(),
                espb.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            id = int.Parse(values[0]);
            sifra = values[1];
            naziv = values[2];
            switch (values[3])
            {
                case "Letnji":
                    semestar = Semestar.Letnji;
                    break;
                case "Zimski":
                    semestar = Semestar.Zimski;
                    break;
            }
            godinaStudija = int.Parse(values[4]);
            profesorId = int.Parse(values[5]);
            espb = uint.Parse(values[6]);
        }
    }
}
