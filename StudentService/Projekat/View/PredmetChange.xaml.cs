using Projekat.Controller;
using Projekat.Model;
using Projekat.Model.DAO;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StudentCreate.xaml
    /// </summary>
    public partial class PredmetChange : Window, INotifyPropertyChanged, IDataErrorInfo, IObserver
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PredmetController _controller;
        private readonly ProfesorController _profesorController;
        private Profesor profesor;
        private Predmet predmet;

        private readonly Predmet SelectedPredmet;
        public ObservableCollection<Profesor> Profesori { get; set; }

        private bool imaProfesora;

        private int id;

        public int Id
        {
            get
            {
                return id; 
            }
            set
            {
                if (id != value)
                {
                    id = value;
                }
            }
        }

        private string sifra;

        public string Sifra
        {
            get
            {
                return sifra;
            }
            set
            {
                if (value != sifra)
                {
                    sifra = value;
                    OnPropertyChanged("Sifra");
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
                    OnPropertyChanged("Naziv");
                }
            }
        }

        private string semestar;

        public string Semestar
        {
            get
            {
                return semestar;
            }
            set
            {
                if (semestar != value)
                {
                    semestar = value;
                    OnPropertyChanged("Semestar");
                }
            }
        }

        private string godinaStudija;

        public string GodinaStudija
        {
            get
            {
                return godinaStudija;
            }
            set
            {
                if (godinaStudija != value)
                {
                    godinaStudija = value;
                    OnPropertyChanged("GodinaStudija");
                }
            }
        }

        private string espb;

        public string Espb
        {
            get
            {
                return espb;
            }
            set
            {
                if (espb != value)
                {
                    espb = value;
                    OnPropertyChanged("GodinaUpisa");
                }
            }
        }

        private string imePrezimeProfesora;
        public string ImePrezimeProfesora
        {
            get
            {
                return imePrezimeProfesora;
            }
            set
            {
                if (imePrezimeProfesora != value)
                {
                    imePrezimeProfesora = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Error => null;

        private Regex _EspbRegex = new Regex(@"^[1-9]{1}[0-9]{0,1}$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Sifra")
                {
                    if (string.IsNullOrEmpty(Sifra))
                        return "Šifra je obavezna";
                }
                else if (columnName == "Naziv")
                {
                    if (string.IsNullOrEmpty(Naziv))
                        return "Naziv je obavezan";
                }
                else if (columnName == "Semestar")
                {
                    if (string.IsNullOrEmpty(Semestar))
                        return "Mora biti izabrana jedna opcija";
                }
                else if (columnName == "GodinaStudija")
                {
                    if (string.IsNullOrEmpty(GodinaStudija))
                        return "Mora biti izabrana jedna opcija";
                }
                else if (columnName == "Espb")
                {
                    if (string.IsNullOrEmpty(Espb))
                        return "Broj ESPB je obavezan";

                    if (Espb[0] == '0')
                        return "Prva cifra ne sme biti 0";

                    Match match = _EspbRegex.Match(Espb);
                    if (!match.Success)
                        return "Broj ESPB mora sadrzati samo 1-2 cifre";

                    if (Convert.ToInt32(Espb) <= 0)
                        return "Broj ESPB mora biti veći od nula";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Sifra", "Naziv", "Semestar", "GodinaStudija", "Espb" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }


        public PredmetChange(PredmetController controller, Predmet selectedPredmet, ProfesorController profesorController)
        {
            InitializeComponent();
            DataContext = this;
            _controller = controller;
            _controller.Subscribe(this);
            imaProfesora = false;
            _profesorController = profesorController;

            profesor = selectedPredmet.profesor;
            predmet = selectedPredmet;

            //_controller.LoadComplexData(_profesorController);

            SelectedPredmet = selectedPredmet;

            this.Id = selectedPredmet.id;
            this.Sifra = selectedPredmet.sifra;
            this.naziv = selectedPredmet.naziv;
            /*if (selectedPredmet.profesor != null)
            {
                this.imePrezimeProfesora = selectedPredmet.imePrezimeProfesora;
                ButtonUkloniProfesora.IsEnabled = true;
                ButtonDodajProfesora.IsEnabled = false;
                imaProfesora = true;
            }
            else
            {
                this.imePrezimeProfesora = null;
                ButtonUkloniProfesora.IsEnabled = false;
                ButtonDodajProfesora.IsEnabled = true;
                imaProfesora = false;
            }*/
            if (SelectedPredmet.profesor != null)
            {
                this.imePrezimeProfesora = SelectedPredmet.profesor.ImePrezime;
                imaProfesora = true;
            }
            else
            {
                ButtonUkloniProfesora.IsEnabled = false;
                imaProfesora = false;
            }
            
            switch (selectedPredmet.semestar)
            {
                case Projekat.Model.Semestar.Letnji:
                    this.Semestar = "Letnji";
                    break;
                case Projekat.Model.Semestar.Zimski:
                    this.Semestar = "Zimski";
                    break;
            }

            switch (selectedPredmet.godinaStudija)
            {
                case 1:
                    this.GodinaStudija = "Prva";
                    break;
                case 2:
                    this.GodinaStudija = "Druga";
                    break;
                case 3:
                    this.GodinaStudija = "Treća";
                    break;
                case 4:
                    this.GodinaStudija = "Četvrta";
                    break;
            }
            this.Espb = selectedPredmet.espb.ToString();

            //TextBox_ImePrezimeProfesora.Text = _controller.GetAllPredmeti().Find(p => p.id == this.Sifra).imePrezimeProfesora;
            //imePrezimeProfesora = selectedPredmet.imePrezimeProfesora;
            //UpdateImePrezimeProfesora();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            if (profesor != SelectedPredmet.profesor)
            {
                _controller.VratiProfesora(profesor, predmet, SelectedPredmet.profesor, _profesorController);
            }
            this.Close();
        }

        private void Button_Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            _controller.Edit(Id, Sifra, Naziv, Semestar, GodinaStudija, Espb);
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsValid)
                ButtonPotvrdi.IsEnabled = true;
            else
                ButtonPotvrdi.IsEnabled = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsValid)
                ButtonPotvrdi.IsEnabled = true;
            else
                ButtonPotvrdi.IsEnabled = false;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsValid)
                ButtonPotvrdi.IsEnabled = true;
            else
                ButtonPotvrdi.IsEnabled = false;
        }

        private void TextBoxProfesor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (imaProfesora == true)
            {
                ButtonDodajProfesora.IsEnabled = false;
                ButtonUkloniProfesora.IsEnabled = true;
            }
            else
            {
                ButtonUkloniProfesora.IsEnabled = false;
            }
        }

        private void ButtonDodajProfesora_Click(object sender, RoutedEventArgs e)
        {
            OdabirProfesora odabirProfesora = new OdabirProfesora(_profesorController, SelectedPredmet, _controller);
            odabirProfesora.Owner = this;
            odabirProfesora.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            odabirProfesora.Show();
        }

        public void Update()
        {
            UpdateImePrezimeProfesora();
        }

        public void UpdateImePrezimeProfesora()
        {
            if (_controller.GetAllPredmeti().Find(p => p.id == this.id) != null)
            {
                TextBox_ImePrezimeProfesora.Text = _controller.GetAllPredmeti().Find(p => p.id == this.id).imePrezimeProfesora;
            }

            if (SelectedPredmet.profesor == null)
            {
                ButtonDodajProfesora.IsEnabled = true;
                ButtonUkloniProfesora.IsEnabled = false;
            }
            else
            {
                ButtonDodajProfesora.IsEnabled = false;
                ButtonUkloniProfesora.IsEnabled = true;
            }
        }

        private void ButtonUkloniProfesora_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da uklonite profesora?", "Uklanjanje profesora", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _controller.ObrisiProfesora(SelectedPredmet, SelectedPredmet.profesor, _profesorController);
            }
        }
    }
}
