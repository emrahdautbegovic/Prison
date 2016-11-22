using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor {
    public partial class MainWindow {
        private void ocistiZatvorenike() {
            PanelBlokA.Children.Clear();
            PanelBlokB.Children.Clear();
            PanelBlokC.Children.Clear();
            PanelSamica.Children.Clear();
            PanelSmrt.Children.Clear();
            PanelArhiv.Children.Clear();
            PanelUslovno.Children.Clear();
        }

        private void ocistiUpravnikeOdjela() {
            panelUpravnici.Children.Clear();
            txtIme.Text = string.Empty;
            txtPrezime.Text = string.Empty;
            txtAdresa.Text = string.Empty;
            txtID.Text = string.Empty;
            txtDatumRodjenjaUO.Text = string.Empty;
            txtBlok.Text = string.Empty;
            txtNapomene.Text = string.Empty;
            txtDatumZaposlenja.Text = string.Empty;
            txtSpol.Text = string.Empty;
            SlikaPodaciUpravnici.Source = null;
        }

        private void ocistiUpravnika() {
            imgSlikaUpravnika.Source = null; ;
            ime.Text = string.Empty;
            prezime.Text = string.Empty;
            adresa.Text = string.Empty;
            datumrodjenja.Text = string.Empty;
            spol.Text = string.Empty;
            napomene.Text = string.Empty;
        }

        private void ocistiCuvare() {
            PanelCuvari.Children.Clear();
            txtImeCuvari.Text = string.Empty;
            txtPrezimeCuvari.Text = string.Empty;
            txtAdresaCuvari.Text = string.Empty;
            txtIDCuvari.Text = string.Empty;
            txtDatumRodjenjaCuvari.Text = string.Empty;
            txtBlokCuvari.Text = string.Empty;
            txtNapomeneCuvari.Text = string.Empty;
            txtDatumZaposlenjaCuvari.Text = string.Empty;
            txtSpolCuvari.Text = string.Empty;
            SlikaPodaciCuvari.Source = null;
            txtZaduzenjaCuvari.Text = string.Empty;
        }

        private void ocistiUposlenike() {
            ocistiCuvare();
            ocistiUpravnikeOdjela();
            ocistiUpravnika();
        }
        private void ocistiPanele() {
            ocistiUposlenike();
            ocistiZatvorenike();
        }
        

    }
}
