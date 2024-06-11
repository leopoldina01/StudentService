using Projekat.Model;
using Projekat.Model.DAO;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Controller
{
    public class ProfesorController
    {
        private readonly ProfesorDAO _profesori;

        public ProfesorController()
        {
            _profesori = new ProfesorDAO();
        }

        public List<Profesor> GetAllProfesors()
        {
            return _profesori.GetAllProfessors();
        }

        public void LoadComplexData(AdresaController adrese, PredmetController predmeti)
        {
            _profesori.LoadComplexData(adrese, predmeti);
        }

        public void DodajPredmet(Profesor profesor, Predmet predmet)
        {
            _profesori.DodajPredmet(profesor, predmet);
        }

        public void ObrisiPredmet(Profesor profesor, Predmet predmet)
        {
            _profesori.ObrisiPredmet(profesor, predmet);
        }

        public void Create(string prezime, string ime, DateTime datumRodj, string adresaStan, string kontakt, string email, string adresaKanc, string brlk, string zvanje, string godineStaza)
        {
            _profesori.AddProfesor(prezime, ime, datumRodj, adresaStan, kontakt, email, adresaKanc, brlk, zvanje, godineStaza);
        }

        public void Edit(int id, string prezime, string ime, DateTime datumRodj, string adresaStan, string kontakt, string email, string adresaKanc, string brlk, string zvanje, string godineStaza)
        {
            _profesori.Edit(id, prezime, ime, datumRodj, adresaStan, kontakt, email, adresaKanc, brlk, zvanje, godineStaza);
        }

        public void Delete(Profesor profesor, PredmetController predmetController, ProfesorController profesorController)
        {
            _profesori.Remove(profesor, predmetController, profesorController);
        }

        public void Save()
        {
            _profesori.Save();
        }

        public void Subscribe(IObserver observer)
        {
            _profesori.Subscribe(observer);
        }
           
    }
}
