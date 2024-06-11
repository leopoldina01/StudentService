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
    public partial class StudentChange : Window, INotifyPropertyChanged, IDataErrorInfo, IObserver
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly StudentController _studentController;
        private readonly PredmetController _predmetController;
        private readonly OcenaController _ocenaController;

        public ObservableCollection<Ocena> Ocene { get; set; }
        public ObservableCollection<Predmet> Predmeti { get; set; }

        public Student SelectedStudent { get; set; }

        public Ocena SelectedOcena { get; set; }

        public Predmet SelectedPredmet { get; set; }

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

        private string prosecnaOcenaStr;

        public string ProsecnaOcenaStr
        {
            get
            {
                return prosecnaOcenaStr;
            }
            set
            {
                if (prosecnaOcenaStr != value)
                {
                    prosecnaOcenaStr = value;
                    OnPropertyChanged("ProsecnaOcenaStr");
                }
            }
        }

        private string ukupnoEspbStr;

        public string UkupnoEspbStr
        {
            get
            {
                return ukupnoEspbStr;
            }
            set
            {
                if (ukupnoEspbStr != value)
                {
                    ukupnoEspbStr = value;
                    OnPropertyChanged("UkupnoEspbStr");
                }
            }
        }

        /*private List<Ocena> polozeniPredmeti;

        public List<Ocena> PolozeniPredmeti 
        {
            get
            {
                return polozeniPredmeti;
            }
            set
            {
                if (polozeniPredmeti != value)
                {
                    polozeniPredmeti = value;
                    //OnPropertyChanged("PolozeniPredmeti");
                }
            }
        }*/

        private List<Predmet> nepolozeniPredmeti;

        public List<Predmet> NepolozeniPredmeti
        {
            get 
            {
                return nepolozeniPredmeti;
            }
            set
            {
                if (nepolozeniPredmeti != value)
                {
                    nepolozeniPredmeti = value;
                    //OnPropertyChanged("NepolozeniPredmeti");
                }
            }
        }

        public string Error => null;

        private Regex _KontaktRegex = new Regex(@"^[0-9]{3}/[0-9]{3}-[0-9]{3}$");
        private Regex _BrIndeksaRegex = new Regex(@"^[A-Z]{2} [0-9]{1,3}[/]{1}[0-9]{4}$");
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
                        return "Format: XX YY/YYYY (X - slovo, Y - cifra)";
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

                    if (_studentController.GetAllStudents().Find(s => s.brIndeksa == brIndeksa) != null && _studentController.GetAllStudents().Find(s => s.brIndeksa == brIndeksa).id != id) 
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


        public StudentChange(StudentController studentController, Student selectedStudent, PredmetController predmetController, OcenaController ocenaController)
        {
            InitializeComponent();
            DataContext = this;
            _studentController = studentController;
            _studentController.Subscribe(this);
            _predmetController = predmetController;
            _predmetController.Subscribe(this);
            SelectedStudent = selectedStudent;
            _ocenaController = ocenaController;
            _ocenaController.Subscribe(this);

            this.Id = selectedStudent.id;
            this.Ime = selectedStudent.ime;
            this.Prezime = selectedStudent.prezime;
            this.DatumRodjenja = selectedStudent.datumRodjenja;
            this.Adresa = selectedStudent.adresa.ToString();
            this.BrTelefona = selectedStudent.brTelefona;
            this.Email = selectedStudent.email;
            this.BrIndeksa = selectedStudent.brIndeksa;
            this.GodinaUpisa = selectedStudent.godinaUpisa.ToString();
            this.ProsecnaOcenaStr = "Prosečna ocena: " + selectedStudent.prosecnaOcena.ToString("0.00");

            switch(selectedStudent.godinaStudija)
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

            switch (selectedStudent.status)
            {
                case Projekat.Model.Status.B:
                    this.Status = "Budžet";
                    break;
                case Projekat.Model.Status.S:
                    this.Status = "Samofinansiranje";
                    break;
            }

            Ocene = new ObservableCollection<Ocena>(selectedStudent.polozeniPredmeti);
            Predmeti = new ObservableCollection<Predmet>(selectedStudent.nepolozeniPredmeti);

            this.UkupnoEspbStr = "Ukupno ESPB: " + _studentController.GetEspb(selectedStudent.id).ToString();
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
            _studentController.Edit(Id, Prezime, Ime, DatumRodjenja, Adresa, BrTelefona, Email, BrIndeksa, GodinaUpisa, GodinaStudija, Status);
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

        private void ButtonPolaganje_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                UpisOcene upisOcene = new UpisOcene(_predmetController, SelectedPredmet, _ocenaController, SelectedStudent, _studentController);
                upisOcene.Owner = this;
                upisOcene.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                upisOcene.Show();
            }
            else
            {
                MessageBox.Show("Odaberite predmet koji želite da ažurirate", "Nije odabran nijedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodavanjePredmeta dodavanjePredmeta = new DodavanjePredmeta(SelectedStudent, _predmetController, _studentController);
            dodavanjePredmeta.Owner = this;
            dodavanjePredmeta.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dodavanjePredmeta.Show();
        }

        public void Update()
        {
            UpdateOcene();
            UpdatePredmeti();
            _studentController.SetProsecnaOcena(SelectedStudent);
            this.ProsecnaOcenaStr = "Prosečna ocena: " + SelectedStudent.prosecnaOcena.ToString("0.00");
            this.UkupnoEspbStr = "Ukupno ESPB: " + _studentController.GetEspb(SelectedStudent.id).ToString();
        }

        private void UpdateOcene()
        {
            Ocene.Clear();
            foreach (Ocena ocena in SelectedStudent.polozeniPredmeti)
            {
                Ocene.Add(ocena);
            }
        }

        private void UpdatePredmeti()
        {
            Predmeti.Clear();
            foreach (Predmet predmet in SelectedStudent.nepolozeniPredmeti)
            {
                Predmeti.Add(predmet);
            }
        }

        private void Button_Ukloni_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da uklonite predmet?", "Uklanjanje predmeta", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _studentController.RemoveNepolozeniPredmet(SelectedStudent, SelectedPredmet);
                }
            }
            else
            {
                MessageBox.Show("Odaberite predmet koji želite da uklonite", "Nije odabran nijedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonPonistiOcenu_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOcena != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da poništite ocenu?", "Poništavanje ocene", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Predmet predmet = SelectedOcena.predmet;
                    _studentController.RemovePolozeniPredmet(SelectedStudent, SelectedOcena);
                    
                    _studentController.AddNepolozeniPredmet(SelectedStudent, predmet);
                }
            }
            else
            {
                MessageBox.Show("Odaberite ocenu koju želite da poništite", "Nije odabrana nijedna ocena", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
