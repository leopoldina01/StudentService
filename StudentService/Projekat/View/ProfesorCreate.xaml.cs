using Projekat.Controller;
using Projekat.Model;
using Projekat.Model.DAO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ProfesorCreate.xaml
    /// </summary>
    public partial class ProfesorCreate : Window, INotifyPropertyChanged, IDataErrorInfo
    {

        private readonly ProfesorController _controller;
        
        public Profesor profesor { get; set; }
        /*private readonly AdresaDAO _adrese;*/

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
        public string Error => null;

        private Regex _KontaktRegex = new Regex(@"^[0-9]{3}/[0-9]{3}-[0-9]{3}$");
        private Regex _BrlkRegex = new Regex(@"^[0-9]{9}$");
        private Regex _AdresaRegex = new Regex(@"^[\p{L}\s]+\s[0-9]{1,3}[a-z]?,\s[\p{L}\s]+,\s[\p{L}\s]+$");
        private Regex _EmailRegex = new Regex(@"^[\w\.]+@\w+\.[\w\.]+$");
        private Regex _GodineStazaRegex = new Regex(@"^[0-9]{1,2}$");

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

                    if (_controller.GetAllProfesors().Find(p => p.brlk == Brlk) != null) 
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
        public ProfesorCreate(ProfesorController controller)
        {
            InitializeComponent();
            DataContext = this;

            _controller = controller;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PotvrdiProfesor_Click(object sender, RoutedEventArgs e)
        {
            //dodavanje za sada ne radi zbog adresa
            if (IsValid)
            {
                _controller.Create(Prezime, Ime, DatumRodj, AdresaStan, Kontakt, Email, AdresaKanc, Brlk, Zvanje, GodineStaza);
                //_controller.Create(profesor);
                Close();
            }
            else
            {
                MessageBox.Show("Profesor se ne može napraviti, jer nisu sva polja validno popunjena.");
            }
        }

        private void CancelProfesor_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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

    }
}
