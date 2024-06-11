using Projekat.Controller;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for UpisOcene.xaml
    /// </summary>
    public partial class UpisOcene : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly PredmetController _controllerPredmet;

        private readonly OcenaController _controllerOcena;
        private readonly StudentController _studentController;

        private Student student;
        private Predmet predmet;

        public event PropertyChangedEventHandler PropertyChanged;
        public Ocena ocena { get; set; }

        private int sifra;

        public int Sifra
        {
            get
            {
                return sifra;
            }
            set
            {
                if (sifra != value)
                {
                    sifra = value;
                    OnPropertyChanged();
                }
            }
        }

        private string naziv;

        public string Naziv
        {
            get
            {
                return naziv;
            }
            set
            {
                if (naziv != value)
                {
                    naziv = value;
                    OnPropertyChanged();
                }
            }
        }

        private double vrednost;

        public double Vrednost
        {
            get
            {
                return vrednost;
            }
            set
            {
                if (vrednost != value)
                {
                    vrednost = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime datumPolaganja;
        public DateTime DatumPolaganja
        {
            get
            {
                return datumPolaganja;
            }
            set
            {
                if (datumPolaganja != value)
                {
                    datumPolaganja = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DatumPolaganja")
                {
                    if (DatumPolaganja == DateTime.MinValue)
                    {
                        return "Datum polaganja je obavezan";
                    }

                    if (DatumPolaganja > DateTime.Now)
                    {
                        return "Datum polaganja ne sme biti u budućnosti";
                    }
                    //treba staviti da datum polaganja ne sme biti pre godine upisa
                    if (DatumPolaganja.Year < 1900)
                    {
                        return "Datum polaganja ne sme biti pre 1900";
                    }
                } else if (columnName == "Vrednost")
                {
                    if (vrednost < 6 || vrednost > 10)
                    {
                        return "Morate izabrati ocenu";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Vrednost", "DatumPolaganja" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public UpisOcene(PredmetController controller, Predmet selectedPredmet, OcenaController controllerOcena, Student selectedStudent, StudentController studentController)
        {
            InitializeComponent();
            DataContext = this;
            _controllerPredmet = controller;
            _controllerOcena = controllerOcena;
            _studentController = studentController;

            predmet = selectedPredmet;
            student = selectedStudent;

            this.Sifra = selectedPredmet.id;
            this.Naziv = selectedPredmet.naziv;
            
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_VrednostChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsValid)
            {
                ButtonPotvrdi.IsEnabled = true;
            } else
            {
                ButtonPotvrdi.IsEnabled = false;
            }
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsValid)
            {
                ButtonPotvrdi.IsEnabled = true;
            }
            else
            {
                ButtonPotvrdi.IsEnabled = false;
            }
        }

        private void ButtonPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            _controllerOcena.Create(student, predmet, vrednost, datumPolaganja);
            _studentController.GetAllStudents().Find(s => s.id == student.id).nepolozeniPredmeti.Remove(predmet);
            ocena = _controllerOcena.GetAllOcene().Find(o => o.polozioId == student.id && o.predmetId == predmet.id);
            _studentController.GetAllStudents().Find(s => s.id == student.id).polozeniPredmeti.Add(ocena);
            _controllerOcena.SaveComplexData(_studentController, _controllerPredmet);
            Close();
        }
    }
}
