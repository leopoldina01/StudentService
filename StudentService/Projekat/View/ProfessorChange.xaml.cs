using Projekat.Controller;
using Projekat.Model;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ProfessorChange.xaml
    /// </summary>
    public partial class ProfessorChange : Window, INotifyPropertyChanged, IDataErrorInfo, IObserver
    {
        private readonly ProfesorController _controller;
        public Profesor selectedProfesor { get; set; }

        private readonly PredmetController _predmetController;

        public Profesor SelectedProfesor { get; set; }
        public Predmet SelectedPredmet { get; set; }

        public ObservableCollection<Predmet> Predmeti { get; set; }
        public ProfessorChange(ProfesorController controller, Profesor selected, PredmetController predmetController)
        {
            InitializeComponent();
            DataContext = this;
            _controller = controller;
            _controller.Subscribe(this);
            SelectedProfesor = selected;

            this.Id = selected.Id;
            this.Prezime = selected.prezime;
            this.Ime = selected.ime;
            this.DatumRodj = selected.datumRodj;
            this.IdAdresaStan = selected.adresaStanId;
            this.AdresaStan = selected.adresaStan.ToString();
            this.IdAdresaKanc = selected.adresaKancId;
            this.AdresaKanc = selected.adresaKanc.ToString();
            this.Kontakt = selected.kontakt;
            this.Email = selected.email;
            this.Brlk = selected.brlk;
            this.Zvanje = selected.zvanje;
            this.GodineStaza = Convert.ToString(selected.godineStaza);

            _predmetController = predmetController;

            Predmeti = new ObservableCollection<Predmet>(SelectedProfesor.predmeti);
        }

        private int id;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private string _prezime;

        public string Prezime
        {
            get => _prezime;
            set
            {
                if (value != _prezime)
                {
                    _prezime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _ime;
        public string Ime
        {
            get => _ime;
            set
            {
                if (value != _ime)
                {
                    _ime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _datumrodj;
        public DateTime DatumRodj
        {
            get => _datumrodj;
            set
            {
                if (value != _datumrodj)
                {
                    _datumrodj = value;
                    OnPropertyChanged("DatumRodj");
                }
            }
        }

        private int _idAdresaStan;
        public int IdAdresaStan
        {
            get => _idAdresaStan;
            set
            {
                if (value != _idAdresaStan)
                {
                    _idAdresaStan = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _adresaStan;
        public string AdresaStan
        {
            get => _adresaStan;
            set
            {
                if (value != _adresaStan)
                {
                    _adresaStan = value; ;
                    OnPropertyChanged();
                }
            }
        }

        private string _kontakt;
        public string Kontakt
        {
            get => _kontakt;
            set
            {
                if (value != _kontakt)
                {
                    _kontakt = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _adresaKanc;
        public string AdresaKanc
        {
            get => _adresaKanc;
            set
            {
                _adresaKanc = value;
                OnPropertyChanged();

            }
        }

        private int _idAdresaKanc;
        public int IdAdresaKanc
        {
            get => _idAdresaKanc;
            set
            {
                if (value != _idAdresaKanc)
                {
                    _idAdresaKanc = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _brlk;
        public string Brlk
        {
            get => _brlk;
            set
            {
                if (value != _brlk)
                {
                    _brlk = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _zvanje;
        public string Zvanje
        {
            get => _zvanje;
            set
            {
                if (value != _zvanje)
                {
                    _zvanje = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _godineStaza;

        public event PropertyChangedEventHandler PropertyChanged;

        public string GodineStaza
        {
            get => _godineStaza;
            set
            {
                if (value != _godineStaza)
                {
                    _godineStaza = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        

        public string Error => null;


        private Regex _KontaktRegex = new Regex(@"^[0-9]{3}/[0-9]{3}-[0-9]{3}$");
        private Regex _BrlkRegex = new Regex(@"^[0-9]{9}$");
        private Regex _AdresaRegex = new Regex(@"^[\p{L}\s]+\s[0-9]{1,3}[a-z]?,\s[\p{L}\s]+,\s[\p{L}\s]+$");
        private Regex _EmailRegex = new Regex(@"^[\w\.]+@\w+\.[\w\.]+$");
        private Regex _GodineStazaRegex = new Regex(@"^[0-9]{1,2}$");


        private readonly string[] _validatedProperties = { "Prezime", "Ime", "DatumRodj", "AdresaStan", "Kontakt", "Email", "AdresaKanc", "Brlk", "Zvanje", "GodineStaza" };

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

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Prezime")
                {
                    if (string.IsNullOrEmpty(Prezime))
                    {
                        return "Prezime je obavezno";
                    }
                }
                else if (columnName == "Ime")
                {
                    if (string.IsNullOrEmpty(Ime))
                    {
                        return "Ime je obavezno";
                    }
                }
                else if (columnName == "DatumRodj")
                {
                    if (DatumRodj == DateTime.MinValue)
                    {
                        return "Datum rođenja je obavezan";
                    }

                    if (DatumRodj > DateTime.Now)
                    {
                        return "Datum rođenja ne sme biti u budućnosti";
                    }

                    if (DatumRodj.Year < 1900)
                    {
                        return "Datum rođenja ne sme biti pre 1900";
                    }
                }
                else if (columnName == "AdresaStan")
                {
                    if (string.IsNullOrEmpty(this.AdresaStan))
                    {
                        return "Adresa je obavezna";
                    }

                    Match match = _AdresaRegex.Match(this.AdresaStan);
                    if (!match.Success)
                    {
                        return "Format: [ULICA] [BROJ], [GRAD], [DRŽAVA]";
                    }
                }
                else if (columnName == "Kontakt")
                {
                    if (string.IsNullOrEmpty(Kontakt))
                    {
                        return "Kontakt je obavezan";
                    }

                    Match match = _KontaktRegex.Match(Kontakt);
                    if (!match.Success)
                    {
                        return "Format: XXX/XXX-XXX";
                    }
                }
                else if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(Email))
                    {
                        return "email je obavezan";
                    }

                    Match match = _EmailRegex.Match(Email);
                    if (!match.Success)
                    {
                        return "email mora biti formata xxx@xxx.xxx";
                    }
                }
                else if (columnName == "AdresaKanc")
                {
                    if (string.IsNullOrEmpty(this.AdresaKanc))
                        return "Adresa je obavezna";

                    Match match = _AdresaRegex.Match(this.AdresaKanc);
                    if (!match.Success)
                        return "Format: [ULICA] [BROJ], [GRAD], [DRŽAVA]";
                }
                else if (columnName == "Brlk")
                {
                    if (string.IsNullOrEmpty(Brlk))
                    {
                        return "Brlk je obavezan";
                    }

                    Match match = _BrlkRegex.Match(Brlk);
                    if (!match.Success)
                    {
                        return "Brlk mora imati 9 cifara";
                    }

                    if (_controller.GetAllProfesors().Find(p => p.brlk == Brlk) != null && _controller.GetAllProfesors().Find(p => p.brlk == Brlk).Id != id)
                    {
                        return "Već postoji profesor sa unetim brlk";
                    }
                }
                else if (columnName == "Zvanje")
                {
                    if (string.IsNullOrEmpty(Zvanje))
                    {
                        return "Zvanje je obavezno";
                    }
                }
                else if (columnName == "GodineStaza")
                {
                    if (string.IsNullOrEmpty(GodineStaza))
                    {
                        return "Godine staža su obavezne";
                    }

                    Match match = _GodineStazaRegex.Match(GodineStaza);
                    if (!match.Success)
                    {
                        return "Godine staža mogu biti samo dvocifrene";
                    }

                    if (Convert.ToInt32(GodineStaza) < 0)
                    {
                        return "Godine staža moraju biti nenegativne";
                    }

                    if (Convert.ToInt32(GodineStaza) > 45)
                    {
                        return "Godine staža ne mogu biti veće od 45";
                    }
                }

                return null;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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

        private void Button_Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            _controller.Edit(Id, Prezime, Ime, DatumRodj, AdresaStan, Kontakt, Email, AdresaKanc, Brlk, Zvanje, GodineStaza);
            this.Close();
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodavanjePredmetaProfesoru dodavanjePredmetaProfesoru = new DodavanjePredmetaProfesoru(SelectedProfesor, _predmetController, _controller);
            dodavanjePredmetaProfesoru.Owner = this;
            dodavanjePredmetaProfesoru.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dodavanjePredmetaProfesoru.Show();
        }

        private void Button_Ukloni_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da uklonite predmet?", "Uklanjanje predmeta", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _controller.ObrisiPredmet(SelectedProfesor, SelectedPredmet);
                }
            }
            else
            {
                MessageBox.Show("Odaberite predmet koji želite da uklonite", "Nije odabran nijedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Update()
        {
            UpdatePredmeti();
        }

        public void UpdatePredmeti()
        {
            Predmeti.Clear();
            foreach (Predmet predmet in SelectedProfesor.predmeti)
            {
                Predmeti.Add(predmet);
            }
        }
    }
}
