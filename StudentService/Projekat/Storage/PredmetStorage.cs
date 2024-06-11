using Projekat.Manager.Serialization;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projekat.Storage
{
    class PredmetStorage
    {
        private readonly string StoragePathPredmeti = string.Format("..{0}..{0}..{0}Data{0}predmeti.csv", Path.DirectorySeparatorChar);
        private readonly Serializer<Predmet> _serializer;

        public PredmetStorage()
        {
            _serializer = new Serializer<Predmet>();
        }

        public List<Predmet> Load()
        {
            return _serializer.FromCSV(StoragePathPredmeti);
        }

        public void Save(List<Predmet> predmeti)
        {
            _serializer.ToCSV(StoragePathPredmeti, predmeti);
        }

    }
}
