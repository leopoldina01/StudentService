using Projekat.Model;
using Projekat.Model.DAO;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Controller
{
    public class AdresaController
    {
        private readonly AdresaDAO _adrese;

        public AdresaController()
        {
            _adrese = new AdresaDAO();
        }

        public void Create(string ulica, string broj, string grad, string drzava)
        {
            _adrese.Add(ulica, broj, grad, drzava);
        }

        public void Delete(Adresa adresa)
        {
            _adrese.Remove(adresa);
        }

        public void Save()
        {
            _adrese.Save();
        }

        public void Subscribe(IObserver observer)
        {
            _adrese.Subscribe(observer);
        }

        public List<Adresa> GetAllAdrese()
        {
            return _adrese.GetAll();
        }
    }
} 
