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
    /// Interaction logic for UnosKorisnika.xaml
    /// </summary>
    public partial class UnosKorisnika : Window {
        private MainWindow mainWindow;

        public UnosKorisnika() {
            InitializeComponent();
        }

        public UnosKorisnika(MainWindow mainWindow) {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btnUnos_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
                ud.create(new User(Convert.ToInt32(txtId.Text), txtUser.Text, txtPass.Password.ToString(), txtMail.Text));
                d.terminirajKonekciju();
                mainWindow.inicijalizirajUsere();
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Problem kod unosa korisnika");
            }
        }

        private void btnKraj_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
