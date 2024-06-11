using Projekat.Model;
using Projekat.Model.DAO;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Controller
{
    public class PredmetController
    {
        private readonly PredmetDAO _predmeti;

        public PredmetController()
        {
            _predmeti = new PredmetDAO();
        }

        public void LoadComplexData(ProfesorController profesori)
        {
            _predmeti.LoadComplexData(profesori);
        }

        public void DodajProfesora(Predmet predmet, Profesor profesor, ProfesorController profesorController)
        {
            _predmeti.DodajProfesora(predmet, profesor, profesorController);
        }

        public void ObrisiProfesora(Predmet predmet, Profesor profesor, ProfesorController profesorController)
        {
            _predmeti.ObrisiProfesora(predmet, profesor, profesorController);
        }

        public void VratiProfesora(Profesor profesor,Predmet predmet,Profesor SelectedPredmetProfesor, ProfesorController _profesorController)
        {
            _predmeti.VratiProfesora(profesor, predmet, SelectedPredmetProfesor, _profesorController);
        }


        public List<Predmet> GetAllPredmeti()
        {
            return _predmeti.GetAllPredmeti();
        }

        public void Create(string sifra, string naziv, string semestar, string godinaStudija, string espb)
        {
            _predmeti.Add(naziv, sifra, semestar, godinaStudija, espb);
        }

        public void Edit(int id, string sifra, string naziv, string semestar, string godinaStudija, string espb)
        {
            _predmeti.Edit(id, sifra, naziv, semestar, godinaStudija, espb);
        }

        public void Delete(Predmet predmet, StudentController studentController, OcenaController ocenaController, ProfesorController profesorController)
        {
            _predmeti.Remove(predmet, studentController, ocenaController, profesorController);
        }

        public void Save()
        {
            _predmeti.Save();
        }

        public void Subscribe(IObserver observer)
        {
            _predmeti.Subscribe(observer);
        }
    }
}
