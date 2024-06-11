using Projekat.Controller;
using Projekat.Model;
using Projekat.Model.DAO;
using System;
using System.Collections.Generic;
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
    public partial class StudentCreate : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly StudentController _controller;

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

        private string ime;

        public string Ime
        {
            get
            {
                return ime;
            }
            set
            {
                if (ime != value)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }

        private string prezime;

        public string Prezime
        {
            get
            {
                return prezime;
            }
            set
            {
                if (prezime != value)
                {
                    prezime = value;
                    OnPropertyChanged("Prezime");
                }
            }
        }

        private DateTime datumRodjenja;

        public DateTime DatumRodjenja
        {
            get
            {
                return datumRodjenja;
            }
            set
            {
                if (datumRodjenja != value)
                {
                    datumRodjenja = value;
                    OnPropertyChanged("DatumRodjenja");
                }
            }
        }

        private string adresa;

        public string Adresa
        {
            get
            {
                return adresa;
            }
            set
            {
                if (adresa != value)
                {
                    adresa = value;
                    OnPropertyChanged("Adresa");
                }
            }
        }

        private string brTelefona;

        public string BrTelefona
        {
            get
            {
                return brTelefona;
            }
            set
            {
                if (brTelefona != value)
                {
                    brTelefona = value;
                    OnPropertyChanged("BrTelefona");
                }
            }
        }

        private string email;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private string brIndeksa;

        public string BrIndeksa
        {
            get
            {
                return brIndeksa;
            }
            set
            {
                if (brIndeksa != value)
                {
                    brIndeksa = value;
                    OnPropertyChanged("BrIndeksa");
                }
            }
        }

        private string godinaUpisa;

        public string GodinaUpisa
        {
            get
            {
                return godinaUpisa;
            }
            set
            {
                if (godinaUpisa != value)
                {
                    godinaUpisa = value;
                    OnPropertyChanged("GodinaUpisa");
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

        private string status;

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        public string Error => null;

        private Regex _KontaktRegex = new Regex(@"^[0-9]{3}/[0-9]{3}-[0-9]{3}$");
        private Regex _BrIndeksaRegex = new Regex(@"^[A-Z]{2} [0-9]{1,3}[-/]{1}[0-9]{4}$");
        private Regex _AdresaRegex = new Regex(@"^[\p{L}\s]+\s[0-9]{1,3}[a-z]?,\s[\p{L}\s]+,\s[\p{L}\s]+$");
        private Regex _EmailRegex = new Regex(@"^[\w\.]+@\w+\.[\w\.]+$");
        private Regex _GodinaUpisaRegex = new Regex(@"^[0-9]{4}$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Ime")
                {
                    if (string.IsNullOrEmpty(Ime))
                        return "Ime je obavezno";
                }
                else if (columnName == "Prezime")
                {
                    if (string.IsNullOrEmpty(Prezime))
                        return "Prezime je obavezno";
                }
                else if (columnName == "DatumRodjenja")
                {
                    if (DatumRodjenja == DateTime.MinValue)
                        return "Datum rođenja je obavezan";

                    if (DatumRodjenja > DateTime.Now)
                        return "Datum rođenja ne sme biti u budućnosti";

                    if (DatumRodjenja.Year < 1900)
                        return "Datum rođenja ne sme biti pre 1900";
                }
                else if (columnName == "Adresa")
                {
                    if (string.IsNullOrEmpty(this.Adresa))
                        return "Adresa je obavezna";

                    Match match = _AdresaRegex.Match(this.Adresa);
                    if (!match.Success)
                        return "Format: [ULICA] [BROJ], [GRAD], [DRŽAVA]";
                }
                else if (columnName == "BrTelefona")
                {
                    if (string.IsNullOrEmpty(BrTelefona))
                        return "Broj telefona je obavezan";

                    Match match = _KontaktRegex.Match(BrTelefona);
                    if (!match.Success)
                        return "Format: XXX/XXX-XXX";
                }
                else if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(Email))
                        return "E-mail adresa je obavezna";

                    Match match = _EmailRegex.Match(Email);
                    if (!match.Success)
                        return "Email mora sadržati [@] i [.]";
                }
                else if (columnName == "BrIndeksa")
                {
                    if (string.IsNullOrEmpty(BrIndeksa))
                        return "Broj indeksa je obavezan";

                    Match match = _BrIndeksaRegex.Match(BrIndeksa);
                    if (!match.Success)
                        return "Format: XX YY-YYYY (X - slovo, Y - cifra)";
                    else
                    {
                        string godinaStr = "";
                        for (int i = 4; i > 0; i--)
                        {
                            godinaStr += BrIndeksa[BrIndeksa.Length - i];
                        }
                        if (Convert.ToInt32(godinaStr) > DateTime.Now.Year)
                        {
                            return "Godina upisa ne sme biti u budućnosti";
                        }
                    }

                    if (_controller.GetAllStudents().Find(s => s.brIndeksa == brIndeksa) != null)
                    {
                        return "Već postoji student sa unetim brojem indeksa";
                    }
                }
                else if (columnName == "GodinaUpisa")
                {
                    if (string.IsNullOrEmpty(GodinaUpisa))
                        return "Godina upisa je obavezna";

                    Match match = _GodinaUpisaRegex.Match(GodinaUpisa);
                    if (!match.Success)
                        return "Godina upisa mora biti 4-cifren broj";

                    if (Convert.ToInt32(GodinaUpisa) > DateTime.Now.Year)
                        return "Godina upisa ne sme biti u budućnosti";
                    else if (Convert.ToInt32(GodinaUpisa) < 1900)
                        return "Godina upisa ne sme biti pre 1900";
                }
                else if (columnName == "GodinaStudija")
                {
                    if (string.IsNullOrEmpty(GodinaStudija))
                        return "Mora biti izabrana jedna opcija";
                }
                else if (columnName == "Status")
                {
                    if (string.IsNullOrEmpty(Status))
                        return "Mora biti izabrana jedna opcija";
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Ime", "Prezime", "DatumRodjenja", "Adresa", "BrTelefona", "Email", "BrIndeksa", "GodinaUpisa", "GodinaStudija", "Status" };

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


        public StudentCreate(StudentController controller)
        {
            InitializeComponent();
            DataContext = this;
            _controller = controller;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            _controller.Create(Prezime, Ime, DatumRodjenja, Adresa, BrTelefona, Email, BrIndeksa, GodinaUpisa, GodinaStudija, Status);
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
    }
}
