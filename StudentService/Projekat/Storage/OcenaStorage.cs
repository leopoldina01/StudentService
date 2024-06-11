using Projekat.Manager.Serialization;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Projekat.Storage
{
    class OcenaStorage
    {
        private readonly string StoragePath = string.Format("..{0}..{0}..{0}Data{0}ocene.csv", Path.DirectorySeparatorChar);
        private readonly string studentiOcene = string.Format("..{0}..{0}..{0}Data{0}studentiOcene.csv", Path.DirectorySeparatorChar);
        private readonly string studentiPredmeti = string.Format("..{0}..{0}..{0}Data{0}studentiPredmeti.csv", Path.DirectorySeparatorChar);

        private readonly Serializer<Ocena> _serializer;
        private readonly Serializer<ManyToManySerialization> _serializerManyToMany;


        public OcenaStorage()
        {
            _serializer = new Serializer<Ocena>();
            _serializerManyToMany = new Serializer<ManyToManySerialization>();
        }

        public List<Ocena> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }

        public void Save(List<Ocena> ocene) 
        {
            _serializer.ToCSV(StoragePath, ocene);
        }

        public void SavePolozeni(List<ManyToManySerialization> polozeni)
        {
            _serializerManyToMany.ToCSV(studentiOcene, polozeni);
        }

        public void SaveNepolozeni(List<ManyToManySerialization> nepolozeni)
        {
            _serializerManyToMany.ToCSV(studentiPredmeti, nepolozeni);
        }
    }
}
