using Projekat.Model;
using Projekat.Model.DAO;
using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Observer;
using Projekat.Storage;

namespace Projekat.Controller
{
    public class OcenaController
    {
        private readonly OcenaDAO _ocene;

        public OcenaController()
        {
            _ocene = new OcenaDAO();
        }

        public void LoadComplexData(StudentController studenti, PredmetController predmeti)
        {
            _ocene.LoadComplexData(studenti, predmeti);
        }

        public void SaveComplexData(StudentController studenti, PredmetController predmeti)
        {
            _ocene.SaveComplexData(studenti, predmeti);
        }

        public List<Ocena> GetAllOcene()
        {
            return _ocene.GetAll();
        }

        public void Create(Student student, Predmet predmet, double vrednost, DateTime datumPolaganje)
        {
            _ocene.Add(student, predmet, vrednost, datumPolaganje);
        }

        public void Edit()
        {
            _ocene.Edit();
        }

        public void Save()
        {
            _ocene.Save();
        }

        public void Delete(Ocena ocena)
        {
            _ocene.Remove(ocena);
        }

        public void Subscribe(IObserver observer)
        {
            _ocene.Subscribe(observer);
        }
    }
}
