using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for ZaboravioPass.xaml
    /// </summary>
    public partial class ZaboravioPass : Window {
        public ZaboravioPass() {
            InitializeComponent();
        }
        private User user = null;

        string mail, pass, noviPass;
        private void saljiMail(User user) {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(mail, pass);
            MailMessage mm = new MailMessage(mail, user.Mail, "Novi password", "Novi password >> " + noviPass);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mm);
            System.Windows.MessageBox.Show("Mail je poslat!");
        }

        private void potvrdiAdmina() {
            Admin a = user as Admin;
            a.Password = noviPass;
            DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
            d.kreirajKonekciju();
            DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
            ad.update(a);
            d.terminirajKonekciju();
        }

        private void potvrdiUsera() {
            user.Password = noviPass;
            DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
            d.kreirajKonekciju();
            DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
            ud.update(user);
            d.terminirajKonekciju();
        }

        private void btnPotrdi_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
                mail = ad.dajAdminovMail();
                pass = ad.dajAdminovPass();
                noviPass = generirajPassword();
                d.terminirajKonekciju();
                if (typeof(Admin) == user.GetType()) potvrdiAdmina();
                else potvrdiUsera();
            } catch (Exception) {

                throw;
            }
        }

        private string generirajPassword() {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 8; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private void btnKraj_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void btnPronadji_Click(object sender, RoutedEventArgs e) {
            try {
                DAL_DAO.DAL d = DAL_DAO.DAL.Instanca;
                d.kreirajKonekciju();
                if (cboxTip.SelectedIndex == 0) {
                    DAL_DAO.DAL.AdminDAO ad = d.getDAO.getAdminDAO();
                    user = ad.dajPoMailu(txtMail.Text);
                    imgSlika.Source = (user as Admin).Slika.Source;
                    txtIme.Text = (user as Admin).Ime;
                    txtPrezime.Text = (user as Admin).Prezime;
                    txtUser.Text = user.UserName;
                    if (System.Windows.Forms.MessageBox.Show("Jeste li ovo vi!", "Provjera",
                                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) btnPotrdi.IsEnabled = true;
                    d.terminirajKonekciju();
                } else if (cboxTip.SelectedIndex == 1) {
                    DAL_DAO.DAL.UserDAO ud = d.getDAO.getUserDAO();
                    user = ud.dajPoMailu(txtMail.Text);
                    imgSlika.Source = ud.dajSlikuUseraPoId(user.Id).Source;
                    txtIme.Text = ud.dajImeUseraPoId(user.Id);
                    txtPrezime.Text = ud.dajPrezimeUseraPoId(user.Id);
                    txtUser.Text = user.UserName;
                    if (System.Windows.Forms.MessageBox.Show("Jeste li ovo vi!", "Provjera",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) btnPotrdi.IsEnabled = true;
                    d.terminirajKonekciju();
                } else System.Windows.Forms.MessageBox.Show("Niste odabrali tip korisnika!");
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
