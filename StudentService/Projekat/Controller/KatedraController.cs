using Projekat.Model;
using Projekat.Model.DAO;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Controller
{
    public class KatedraController
    {
        private readonly KatedraDAO _katedre;

        public KatedraController()
        {
            _katedre = new KatedraDAO();
        }

        public void LoadComplexData(ProfesorController profesori)
        {
            _katedre.LoadComplexData(profesori);
        }

        public void SetSefKatedre(Katedra katedra, Profesor profesor, ProfesorController profesorController, Profesor stariSef)
        {
            _katedre.SetSefKatedre(katedra, profesor, profesorController, stariSef);
        }

        public List<Katedra> GetAllKatedre()
        {
            return _katedre.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _katedre.Subscribe(observer);
        }
    }
}
