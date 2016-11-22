using System;
using System.Collections.Generic;
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


namespace Zatvor {
    /// <summary>
    /// Interaction logic for OsobaKontrola.xaml
    /// </summary>
    public partial class OsobaKontrola : UserControl {
        public OsobaKontrola() {
            InitializeComponent();
        }
        public bool Klik = false;
        private Osoba osoba;
        private User user;
        public OsobaKontrola(string ime, Image slika) {
            InitializeComponent();
            imgSlika.Source = slika.Source;
            txtImeOsobe.Text = ime;
                      
        }

        public OsobaKontrola(Admin a) {
            user = a;
            InitializeComponent();
            imgSlika.Source = a.Slika.Source;
            txtImeOsobe.Text = a.UserName;
        }

        public OsobaKontrola(User u) {            
            user = u;
            InitializeComponent();
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
                imgSlika.Source = ud.dajSlikuUseraPoId(u.Id).Source;
                d.terminirajKonekciju();            
            txtImeOsobe.Text = u.UserName;            
        }

        public Osoba dajOsobu() { return osoba; }
        public User dajUsera() { return user; }
        public OsobaKontrola(Osoba o) {
            InitializeComponent();
            osoba = o;
            imgSlika.Source = osoba.Slika.Source;
            txtImeOsobe.Text = osoba.Prezime + " " + osoba.Ime;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e) {
            this.BorderBrush = Brushes.Black;
            Klik = true;
        }       
    }
}
