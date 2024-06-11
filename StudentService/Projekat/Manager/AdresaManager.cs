using System;
using System.Collections.Generic;
using System.Text;
using Projekat.Model;

namespace Projekat.Manager
{
    class AdresaManager
    {
        public ParentManager parentManager { get; set; }

        private readonly string fileName = "adrese.txt";

        public AdresaManager(ParentManager parentManager)
        {
            this.parentManager = parentManager;
        }

        private void saveAdrese()
        {
            parentManager.adresaSerializer.ToCSV(fileName, parentManager.adrese);
        }

        private int generateId()
        {
            if (parentManager.adrese.Count == 0)
            {
                return 0;
            }
            return parentManager.adrese[parentManager.adrese.Count - 1].Id + 1;
        }

        public Adresa addAdresa(Adresa adresa)
        {
            adresa.Id = generateId();
            parentManager.adrese.Add(adresa);
            saveAdrese();
            return adresa;
        }

        public Adresa getAdresaById(int id)
        {
            return parentManager.adrese.Find(a => a.Id == id);
        }

        public Adresa updateAdresa(Adresa adresa)
        {
            Adresa oldAdresa = getAdresaById(adresa.Id);
            if (oldAdresa == null)
            {
                return null;
            }

            oldAdresa.Ulica = adresa.Ulica;
            oldAdresa.Broj = adresa.Broj;
            oldAdresa.Grad = adresa.Grad;
            oldAdresa.Drzava = adresa.Drzava;

            saveAdrese();
            return oldAdresa;
        }

        public Adresa RemoveAdresa(int id)
        {
            Adresa adresa = getAdresaById(id);
            if (adresa == null)
            {
                return null;
            }

            parentManager.adrese.Remove(adresa);
            saveAdrese();
            return adresa;
        }

        public List<Adresa> GetAllAdrese()
        {
            return parentManager.adrese;
        }
    }
}
