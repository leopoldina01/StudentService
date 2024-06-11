using Projekat.Manager.Serialization;
using Projekat.Model.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Projekat.Model
{
    public class Profesor : Serializable
    {
        public AdresaDAO _adrese;
        public int Id { get; set; }
        public string prezime { get; set; }
        
        public string ime { get; set; }
        public DateTime datumRodj { get; set; }
        public Adresa adresaStan { get; set; }
        public int adresaStanId { get; set; } //id adrese dodat
        public string kontakt { get; set; }
        public string email { get; set; }
        public Adresa adresaKanc { get; set; }
        public int adresaKancId { get; set; } //id adrese dodat
        public string brlk { get; set; }
        public string zvanje { get; set; }
        public int godineStaza { get; set; }
        public int idKatedre { get; set; }

        public string ImePrezime
        {
            get
            {
                return ime + " " + prezime;
            }
        }
        
        public List<Predmet> predmeti { get; set; } //dodata je klasa predmet

        public Profesor()
        {
            predmeti = new List<Predmet>();
            adresaKancId = -1;
            adresaStanId = -1;
            idKatedre = -1;
            //_adrese = new AdresaDAO();
        }

        public Profesor(int id, string Prezime, string Ime, DateTime DatumRodj, Adresa adresaStan, string Kontakt, string Email, Adresa adresaKanc, string BRLK, string Zvanje, int GodinaStaza)/*List<Predmet> Predmeti*/
        {
            Id = id;
            prezime = Prezime;
            ime = Ime;
            datumRodj = DatumRodj;
            this.adresaStan = adresaStan;
            //adresaStan = new Adresa(); //kasnije cemo videti kako da dodamo adresu
            //ici ce mozda preko novih adresaDAO klasa i ostalog ili nekako drugacije
            adresaStanId = adresaStan.Id; //treba zbog funkcija toCSV i FromCSV ja mislim
            kontakt = Kontakt;
            email = Email;
            this.adresaKanc = adresaKanc;
            adresaKancId = adresaKanc.Id; //isto kao i za prethodni id
            brlk = BRLK;
            zvanje = Zvanje;
            godineStaza = GodinaStaza;
            predmeti = new List<Predmet>();
            idKatedre = -1;
            //predmeti = Predmeti;
        }

        public override string ToString()
        {
            return ImePrezime;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                prezime,
                ime,
                datumRodj.ToString(),
                adresaStanId.ToString(), //koristimo id za adresu
                kontakt,
                email,
                adresaKancId.ToString(), //opet id
                brlk.ToString(),
                zvanje,
                godineStaza.ToString(),
                idKatedre.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            prezime = values[1];
            ime = values[2];
            datumRodj = DateTime.Parse(values[3]);
            adresaStanId = int.Parse(values[4]); //koristice se id za adrese
            kontakt = values[5];
            email = values[6];
            adresaKancId = int.Parse(values[7]);
            brlk = values[8];
            zvanje = values[9];
            godineStaza = int.Parse(values[10]);
            idKatedre = int.Parse(values[11]);
        }

    }
}
