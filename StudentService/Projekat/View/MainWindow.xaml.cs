using Projekat.Controller;
using Projekat.Manager;
using Projekat.Model;
using Projekat.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver, INotifyPropertyChanged
    {

        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        private readonly StudentController _controllerStudent;
        private readonly ProfesorController _controllerProfesor;
        private readonly PredmetController _controllerPredmet;
        private readonly AdresaController _controllerAdresa;
        private readonly OcenaController _controllerOcena;
        private readonly KatedraController _controllerKatedra;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Student> Studenti { get; set; }
        public ObservableCollection<Predmet> Predmeti { get; set; }
        public ObservableCollection<Profesor> Profesori { get; set; }
        public ObservableCollection<Katedra> Katedre { get; set; }

        public Student SelectedStudent { get; set; }
        public Profesor SelectedProfesor { get; set; }
        public Predmet SelectedPredmet { get; set; }
        public Katedra SelectedKatedra { get; set; }

        public Boolean profesoriFocus { get; set; }
        public Boolean predmetiFocus { get; set; }
        public Boolean studentiFocus { get; set; }

        private string studentskaSluzbaStr;

        private string selektovaniTab;
        public string SelektovaniTab
        {
            get
            {
                return selektovaniTab;
            }
            set
            {
                if (value != selektovaniTab)
                {
                    selektovaniTab = value;
                    OnPropertyChanged("SelektovaniTab");
                }
            }
        }

        private string dateTimeStr;
        public string DateTimeStr
        {
            get
            {
                return dateTimeStr;
            }
            set
            {
                dateTimeStr = value;
                OnPropertyChanged("DateTimeStr");
            }
        }

        private string _searchText;

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;

                OnPropertyChanged("SearchText");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //lokalizacija
            app = (App)Application.Current;
            app.ChangeLanguage(SRB);

            //flagovi za tabove
            studentiFocus = true;
            profesoriFocus = false;
            predmetiFocus = false;
            studentskaSluzbaStr = "Studentska služba - ";
            SelektovaniTab = studentskaSluzbaStr + "Studenti";


            //dimenzije ekrana prilikom otvaranja
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Width = SystemParameters.FullPrimaryScreenWidth * 3 / 4;
            Height = SystemParameters.FullPrimaryScreenHeight * 3 / 4;

            //funkcija za updateovanje vremena
            UpdateTime();

            //ToolBar dimenzije
            ToolBarTrayMain.Width = Width;
            ToolBarButtons.Width = Width;
            Button_Add.Width = Width / 32;
            Button_Edit.Width = Width / 32;
            Button_Delete.Width = Width / 32;
            TextBox_Search.Width = Width / 6.6;
            Button_Search.Width = Width / 32;
            SeparatorToolBar.Width = Width - (Width / 32 + Width / 32 + Width / 32 + Width / 6.6 + Width / 32 + Width / 32);

            //GridRow dimenzije
            grid.RowDefinitions.ElementAt(0).Height = new GridLength(0.04 * this.Height);
            grid.RowDefinitions.ElementAt(1).Height = new GridLength(0.05 * this.Height);
            grid.RowDefinitions.ElementAt(2).Height = new GridLength(0.8 * this.Height);
            grid.RowDefinitions.ElementAt(3).Height = new GridLength(0.05 * this.Height);

            //Instanciranje svih kontrolera
            _controllerStudent = new StudentController();
            _controllerProfesor = new ProfesorController();
            _controllerPredmet = new PredmetController();
            _controllerAdresa = new AdresaController();
            _controllerOcena = new OcenaController();
            _controllerKatedra = new KatedraController();

            //Ucitavanje kompleksnih podataka
            _controllerStudent.LoadComplexData(_controllerAdresa, _controllerOcena, _controllerPredmet);
            _controllerOcena.LoadComplexData(_controllerStudent, _controllerPredmet);
            _controllerPredmet.LoadComplexData(_controllerProfesor);
            _controllerProfesor.LoadComplexData(_controllerAdresa, _controllerPredmet);
            _controllerKatedra.LoadComplexData(_controllerProfesor);

            //Dodavanje main prozora u listu observera
            _controllerStudent.Subscribe(this);
            _controllerProfesor.Subscribe(this);
            _controllerPredmet.Subscribe(this);
            _controllerAdresa.Subscribe(this);
            _controllerOcena.Subscribe(this);
            _controllerKatedra.Subscribe(this);


            //ispisivanje svih studenata
            Studenti = new ObservableCollection<Student>(_controllerStudent.GetAllStudents());

            //ispisivanje svih profesora
            Profesori = new ObservableCollection<Profesor>(_controllerProfesor.GetAllProfesors());

            //ispisivanje svih predmeta
            Predmeti = new ObservableCollection<Predmet>(_controllerPredmet.GetAllPredmeti());

            //ispisivanje svih katedri
            Katedre = new ObservableCollection<Katedra>(_controllerKatedra.GetAllKatedre());
        }

        private void UpdateStudentiList()
        {
            Studenti.Clear();
            foreach(var student in _controllerStudent.GetAllStudents())
            {
                Studenti.Add(student);
            }
        }

        private void UpdateProfesorList()
        {
            Profesori.Clear();
            foreach (var profesor in _controllerProfesor.GetAllProfesors())
            {
                Profesori.Add(profesor);
            }
        }

        private void UpdatePredmetiList()
        {
            Predmeti.Clear();
            foreach (var predmet in _controllerPredmet.GetAllPredmeti())
            {
                Predmeti.Add(predmet);
            }
        }

        private void UpdateKatedreList()
        {
            Katedre.Clear();
            foreach (Katedra katedra in _controllerKatedra.GetAllKatedre())
            {
                Katedre.Add(katedra);
            }
        }

        public void Update()
        {
            UpdateStudentiList();
            UpdateProfesorList();
            UpdatePredmetiList();
            UpdateKatedreList();
        }

        private void Student_Focus(object sender, RoutedEventArgs e)
        {
            studentiFocus = true;
            predmetiFocus = false;
            profesoriFocus = false;
            TabItem ti = tabovi.SelectedItem as TabItem;
            SelektovaniTab = studentskaSluzbaStr + ti.Header as string;
            Button_Add.IsEnabled = true;
            Button_Delete.IsEnabled = true;
            MenuItem_New.IsEnabled = true;
            MenuItem_Delete.IsEnabled = true;
        }

        private void Profesor_Focus(object sender, RoutedEventArgs e)
        {
            studentiFocus = false;
            profesoriFocus = true;
            predmetiFocus = false;
            TabItem ti = tabovi.SelectedItem as TabItem;
            SelektovaniTab = studentskaSluzbaStr + ti.Header as string;
            Button_Add.IsEnabled = true;
            Button_Delete.IsEnabled = true;
            MenuItem_New.IsEnabled = true;
            MenuItem_Delete.IsEnabled = true;
        }

        private void Predmet_Focus(object sender, RoutedEventArgs e)
        {
            studentiFocus = false;
            profesoriFocus = false;
            predmetiFocus = true;
            TabItem ti = tabovi.SelectedItem as TabItem;
            SelektovaniTab = studentskaSluzbaStr + ti.Header as string;
            Button_Add.IsEnabled = true;
            Button_Delete.IsEnabled = true;
            MenuItem_New.IsEnabled = true;
            MenuItem_Delete.IsEnabled = true;
        }

        private void Katedra_Focus(object sender, RoutedEventArgs e)
        {
            studentiFocus = false;
            profesoriFocus = false;
            predmetiFocus = false;
            TabItem ti = tabovi.SelectedItem as TabItem;
            SelektovaniTab = studentskaSluzbaStr + ti.Header as string;
            Button_Add.IsEnabled = false;
            Button_Delete.IsEnabled = false;
            MenuItem_New.IsEnabled = false;
            MenuItem_Delete.IsEnabled = false;
        }

        private void Menu_New_Click(object sender, RoutedEventArgs e)
        {
            if (profesoriFocus)
            {
                ProfesorCreate profesorCreate = new ProfesorCreate(_controllerProfesor);
                profesorCreate.Owner = this;
                profesorCreate.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                profesorCreate.Show();
            }
            else if (studentiFocus)
            {
                StudentCreate studentCreate = new StudentCreate(_controllerStudent);
                studentCreate.Owner = this;
                studentCreate.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                studentCreate.Show();
            }
            else if (predmetiFocus)
            {
                PredmetCreate predmetCreate = new PredmetCreate(_controllerPredmet);
                predmetCreate.Owner = this;
                predmetCreate.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                predmetCreate.Show();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ToolBarTrayMain.Width = Width;
            ToolBarButtons.Width = Width;
            SeparatorToolBar.Width = Width - (Width / 32 + Width / 32 + Width / 32 + Width / 6.6 + Width / 32 + Width / 32);
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Menu_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Menu_Edit_Click(object sender, RoutedEventArgs e)
        {

            if (profesoriFocus)
            {
                if (SelectedProfesor != null)
                {
                    ProfessorChange profesorChange = new ProfessorChange(_controllerProfesor, SelectedProfesor, _controllerPredmet);
                    profesorChange.Owner = this;
                    profesorChange.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    profesorChange.Show();
                }
                else
                {
                    MessageBox.Show("Odaberite profesora kojeg zelite da azurirate", "Nije odabran nijedan profesor", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (studentiFocus)
            {
                if (SelectedStudent != null)
                {
                    StudentChange studentChange = new StudentChange(_controllerStudent, SelectedStudent, _controllerPredmet, _controllerOcena);
                    studentChange.Owner = this;
                    studentChange.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    studentChange.Show();
                }
                else
                {
                    MessageBox.Show("Odaberite studenta kojeg želite da ažurirate", "Nije odabran nijedan student", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (predmetiFocus)
            {
                if (SelectedPredmet != null)
                {
                    PredmetChange predmetChange = new PredmetChange(_controllerPredmet, SelectedPredmet, _controllerProfesor);
                    predmetChange.Owner = this;
                    predmetChange.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    predmetChange.Show();
                }
                else
                {
                    MessageBox.Show("Odaberite predmet koji želite da ažurirate", "Nije odabran nijedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (tabovi.SelectedIndex == 3)
            {
                if (SelectedKatedra != null)
                {
                    DodavanjeSefa dodavanjeSefa = new DodavanjeSefa(SelectedKatedra, _controllerProfesor, _controllerKatedra);
                    dodavanjeSefa.Owner = this;
                    dodavanjeSefa.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    dodavanjeSefa.Show();
                }
                else
                {
                    MessageBox.Show("Odaberite katedru koju želite da ažurirate", "Nije odabrana nijedna katedra", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private MessageBoxResult ConfirmDeletion()
        {

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            string sCaption = "Porvrda brisanja";

            if (profesoriFocus)
            {
                string sMessageBoxText = $"Da li ste sigurni da želite da izbrišete profesora\n{SelectedProfesor}";

                MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return result;
            }
            else if (predmetiFocus)
            {
                string sMessageBoxText = $"Da li ste sigurni da želite da izbrišete predmet\n{SelectedPredmet}";

                MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return result;
            }
            else
            {
                string sMessageBoxText = $"Da li ste sigurni da želite da izbrišete studenta\n{SelectedStudent}";

                MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return result;
            }
        }

        private void Menu_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (profesoriFocus)
            {
                if (SelectedProfesor != null)
                {
                    if (_controllerKatedra.GetAllKatedre().Find(k => k.sefId == SelectedProfesor.Id) != null)
                    {
                        MessageBox.Show("Ne možete obrisati profesora", "Profesor je šef katedre", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBoxResult result = ConfirmDeletion();

                        if (result == MessageBoxResult.Yes)
                        {
                            _controllerProfesor.Delete(SelectedProfesor, _controllerPredmet, _controllerProfesor);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Odaberite profesora kojeg želite da izbrišete", "Nije odabran ni jedan profesor", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (predmetiFocus)
            {
                if (SelectedPredmet != null)
                {
                    MessageBoxResult result = ConfirmDeletion();

                    if (result == MessageBoxResult.Yes)
                    {
                        Predmet predmet = SelectedPredmet;
                        _controllerPredmet.Delete(SelectedPredmet, _controllerStudent, _controllerOcena, _controllerProfesor);
                    }
                }
                else
                {
                    MessageBox.Show("Odaberite predmet koji želite da izbrišete", "Nije odabran ni jedan predmet", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (studentiFocus)
            {
                if (SelectedStudent != null)
                {
                    MessageBoxResult result = ConfirmDeletion();

                    if (result == MessageBoxResult.Yes)
                    {
                        _controllerStudent.Delete(SelectedStudent);
                    }
                }
                else
                {
                    MessageBox.Show("Odaberite studenta kojeg želite da izbrišete", "Nije odabran ni jedan student", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void Menu_Open_Studenti_Click(object sender, RoutedEventArgs e)
        {
            tabovi.SelectedIndex = 0;
        }
        private void Menu_Open_Predmeti_Click(object sender, RoutedEventArgs e)
        {
            tabovi.SelectedIndex = 2;
        }
        private void Menu_Open_Profesori_Click(object sender, RoutedEventArgs e)
        {
            tabovi.SelectedIndex = 1;
        }

        private void Menu_Open_Katedre_Click(object sender, RoutedEventArgs e)
        {
            tabovi.SelectedIndex = 3;
        }
        private void Menu_Save_Click(object sender, RoutedEventArgs e)
        {
            _controllerStudent.Save();
            _controllerProfesor.Save();
            _controllerPredmet.Save();
            _controllerAdresa.Save();
        }

        private async void UpdateTime()
        {
            DateTimeStr = DateTime.Now.ToString("HH:mm dd.MM.yyyy");
            await Task.Delay(1000);
            UpdateTime();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
                Menu_New_Click(sender, e);
            /*else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                Menu_Save_Click(sender, e);*/
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C))
                Menu_Close_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.E))
                Menu_Edit_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
                Menu_Delete_Click(sender, e);
            /*else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                Menu_About_Click(sender, e);*/
            else if (Keyboard.IsKeyDown(Key.Enter))
                ButtonSerach_Click(sender, e);
        }

        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (profesoriFocus)
            {
                Profesori.Clear();

                    foreach (var profesor in _controllerProfesor.GetAllProfesors())
                    {
                        Profesori.Add(profesor);
                    }
            }
            else if (predmetiFocus)
            {
                Predmeti.Clear();

                    foreach (var predmet in _controllerPredmet.GetAllPredmeti())
                    {
                        Predmeti.Add(predmet);
                    }
            }
            else if (studentiFocus)
            {
                Studenti.Clear();

                    foreach (var student in _controllerStudent.GetAllStudents())
                    {
                        Studenti.Add(student);
                    }
            }
        }

        private void ButtonSerach_Click(object sender, RoutedEventArgs e)
        {
            if (profesoriFocus)
            {
                Profesori.Clear();

                if (string.IsNullOrEmpty(SearchText))
                {
                    foreach (var profesor in _controllerProfesor.GetAllProfesors())
                    {
                        Profesori.Add(profesor);
                    }
                }
                else
                {
                    string trimovanje = SearchText.Trim();
                    string[] unete_reci = trimovanje.Split(',');
                    int broj_reci = unete_reci.Count();
                    string prva_rec = "";
                    string druga_rec = "";


                    if (broj_reci == 1)
                    {
                        prva_rec = unete_reci[0].Trim();
                    }
                    else if (broj_reci == 2)
                    {
                        prva_rec = unete_reci[0].Trim();
                        druga_rec = unete_reci[1].Trim();
                    }

                    foreach (Profesor p in _controllerProfesor.GetAllProfesors())
                    {
                        if (broj_reci == 1)
                        {
                            if (p.prezime.ToUpper().Contains(prva_rec.ToUpper()))
                            {
                                Profesori.Add(p);
                            }
                        }
                        else if (broj_reci == 2)
                        {
                            if (p.prezime.ToUpper().Contains(prva_rec.ToUpper()) && p.ime.ToUpper().Contains(druga_rec.ToUpper()))
                            {
                                Profesori.Add(p);
                            }
                        }
                    }

                    if (Profesori.Count == 0) 
                    {
                        MessageBox.Show("Nije pronađen nijedan rezultat", "Pokušajte ponovo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else if (predmetiFocus)
            {
                Predmeti.Clear();

                if (string.IsNullOrEmpty(SearchText))
                {
                    foreach (var predmet in _controllerPredmet.GetAllPredmeti())
                    {
                        Predmeti.Add(predmet);
                    }
                }
                else
                {
                    string trimovanje = SearchText.Trim();
                    string[] unete_reci = trimovanje.Split(',');
                    int broj_reci = unete_reci.Count();
                    string prva_rec = "";
                    string druga_rec = "";

                    if (broj_reci == 1)
                    {
                        prva_rec = unete_reci[0].Trim();
                    }
                    else if (broj_reci == 2)
                    {
                        prva_rec = unete_reci[0].Trim();
                        druga_rec = unete_reci[1].Trim();
                    }

                    foreach (Predmet p in _controllerPredmet.GetAllPredmeti())
                    {
                        if (broj_reci == 1)
                        {
                            if (p.naziv.ToUpper().Contains(prva_rec.ToUpper()))
                            {
                                Predmeti.Add(p);
                            }
                        }
                        else if (broj_reci == 2)
                        {
                            if (p.naziv.ToUpper().Contains(prva_rec.ToUpper()) && p.sifra.ToUpper().Contains(druga_rec.ToUpper()))
                            {
                                Predmeti.Add(p);
                            }
                        }
                    }

                    if (Predmeti.Count == 0)
                    {
                        MessageBox.Show("Nije pronađen nijedan rezultat", "Pokušajte ponovo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else if (studentiFocus)
            {
                Studenti.Clear();

                if (string.IsNullOrEmpty(SearchText))
                {
                    foreach (var student in _controllerStudent.GetAllStudents())
                    {
                        Studenti.Add(student);
                    }
                }
                else
                {
                        string trimovanje = SearchText.Trim();
                        string[] unete_reci = trimovanje.Split(',');
                        int broj_reci = unete_reci.Count();
                        string prva_rec = "";
                        string druga_rec = "";
                        string treca_rec = "";

                        if (broj_reci == 1)
                        {
                        prva_rec = unete_reci[0].Trim();

                        } else if (broj_reci == 2)
                        {
                        prva_rec = unete_reci[0].Trim();
                        druga_rec = unete_reci[1].Trim();

                        } else if (broj_reci == 3)
                        {
                        prva_rec = unete_reci[0].Trim();
                        druga_rec = unete_reci[1].Trim();
                        treca_rec = unete_reci[2].Trim();

                        }


                        foreach (Student p in _controllerStudent.GetAllStudents())
                        {
                            if (broj_reci == 1)
                            {
                                if (p.prezime.ToUpper().Contains(prva_rec.ToUpper()))
                                {
                                    Studenti.Add(p);
                                }
                            } else if (broj_reci == 2)
                            {
                                if (p.prezime.ToUpper().Contains(prva_rec.ToUpper()) && p.ime.ToUpper().Contains(druga_rec.ToUpper()))
                                {
                                    Studenti.Add(p);
                                }
                            } else if (broj_reci == 3)
                            {
                                if (p.brIndeksa.ToUpper().Contains(prva_rec.ToUpper()) && p.ime.ToUpper().Contains(druga_rec.ToUpper()) && p.prezime.ToUpper().Contains(treca_rec.ToUpper()))
                                {
                                    Studenti.Add(p);
                                }
                            }
                        }

                    if (Studenti.Count == 0)
                    {
                        MessageBox.Show("Nije pronađen nijedan rezultat", "Pokušajte ponovo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void Menu_Language_Click(object sender, RoutedEventArgs e)
        {
            if ((string) MenuItem_Language.Header == "SR")
            {
                app.ChangeLanguage(SRB);
                studentskaSluzbaStr = "Studentska služba - ";
                TabItem ti = tabovi.SelectedItem as TabItem;
                SelektovaniTab = studentskaSluzbaStr + ti.Header as string;
            }
            else
            {
                app.ChangeLanguage(ENG);
                studentskaSluzbaStr = "Student service - ";
                TabItem ti = tabovi.SelectedItem as TabItem;
                SelektovaniTab = studentskaSluzbaStr + ti.Header as string;
            }
        }
    }
}
