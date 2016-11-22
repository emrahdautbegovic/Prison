using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Zatvor {
    /// <summary>
    /// Interaction logic for UnosAdmina.xaml
    /// </summary>
    public partial class UnosAdmina : Window {
        private MainWindow mainWindow;
        private Admin admin;

        private bool zaUredjivanje = false;
        public UnosAdmina() {
            InitializeComponent();
        }

        public UnosAdmina(MainWindow mainWindow) {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        public UnosAdmina(Admin admin, MainWindow mainWindow) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.admin = admin;
            zaUredjivanje = true;
            btnDodaj.Content = "Promijeni";
            tboxIme.Text = admin.Ime;
            tboxPrezime.Text = admin.Prezime;
            tboxUser.Text = admin.UserName;
            tboxPass.Password = admin.Password.ToString();
            tboxMail.Text = admin.Mail;
            imgSlika.Source = admin.Slika.Source;
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
                if (!zaUredjivanje)
                    ad.create(new Admin(0, tboxIme.Text, tboxPrezime.Text, tboxUser.Text, tboxPass.Password.ToString(), tboxMail.Text, TipAdmina.Sporedni, imgSlika));
                else ad.update(new Admin(0, tboxIme.Text, tboxPrezime.Text, tboxUser.Text, tboxPass.Password.ToString(), tboxMail.Text, TipAdmina.Sporedni, imgSlika));
                d.terminirajKonekciju();
                mainWindow.inicijalizirajAdmine();
                if(!zaUredjivanje) System.Windows.MessageBox.Show("Admin unešen!");
                else System.Windows.MessageBox.Show("Podaci su promijenjeni");                
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);                
            }
        }

        private void btnKraj_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void btnSlika_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog o = new OpenFileDialog();
                bool? ok = o.ShowDialog();
                if (ok ?? true) {
                    imgSlika.Source = new BitmapImage(new Uri(o.FileName));
                }
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
