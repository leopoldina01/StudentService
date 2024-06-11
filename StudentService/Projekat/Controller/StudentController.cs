using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Model;
using Projekat.Model.DAO;
using Projekat.Observer;
using Projekat.Storage;

namespace Projekat.Controller
{
    public class StudentController
    {
        private readonly StudentDAO _students;

        public StudentController()
        {
            _students = new StudentDAO();
        }

        public void LoadComplexData(AdresaController adrese, OcenaController ocene, PredmetController predmeti)
        {
            _students.LoadComplexData(adrese, ocene, predmeti);
        }

        public void AddNepolozeniPredmet(Student student, Predmet predmet)
        {
            _students.AddNepolozeniPredmet(student, predmet);
        }

        public void RemovePolozeniPredmet(Student student, Ocena ocena)
        {
            _students.RemovePolozeniPredmet(student, ocena);
        }

        public void RemoveNepolozeniPredmet(Student student, Predmet predmet)
        {
            _students.RemoveNepolozeniPredmet(student, predmet);
        }

        public void SetProsecnaOcena(Student student)
        {
            _students.SetProsecnaOcena(student);
        }

        public uint GetEspb(int id)
        {
            return _students.GetEspb(id);
        }

        public List<Student> GetAllStudents()
        {
            return _students.GetAll();
        }

        /*public List<Ocena> GetAllOcene(int id)
        {
            return _students.GetAllOcene(id);
        }*/

        public void Create(string prezime, string ime, DateTime datumRodjenja, string adresa, string brTelefona, string email, string brIndeksa, string godinaUpisa, string godinaStudija, string status)
        {
            _students.Add(prezime, ime, datumRodjenja, adresa, brTelefona, email, brIndeksa, godinaUpisa, godinaStudija, status);
        }

        public void Edit(int id, string prezime, string ime, DateTime datumRodjenja, string adresa, string brTelefona, string email, string brIndeksa, string godinaUpisa, string godinaStudija, string status)
        {
            _students.Edit(id, prezime, ime, datumRodjenja, adresa, brTelefona, email, brIndeksa, godinaUpisa, godinaStudija, status);
        }

        public void Save()
        {
            _students.Save();
        }

        public void Delete(Student student)
        {
            _students.Remove(student);
        }

        public void Subscribe(IObserver observer)
        {
            _students.Subscribe(observer);
        }
    }
}
