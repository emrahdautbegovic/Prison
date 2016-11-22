using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zatvor {
    public class Izvjestaj {
        public Osoba Podnosioc { get; set; }
        public DateTime DatumPodnosenja { get; set; }
        public List<String> Podaci { get; set; }
        public Izvjestaj() {
            Podaci = new List<string>();
        }
        public Izvjestaj(Osoba podnosioc, DateTime datumPodnosenja, List<string> podaci) {
            Podaci = podaci;
            Podnosioc = podnosioc;
            DatumPodnosenja = datumPodnosenja;
        }

        public void PrintajIzvjestaj(System.Windows.Controls.RichTextBox t) {
            t.AppendText("Podnosioc: " + Podnosioc.ToString() + Environment.NewLine);                    
            t.AppendText("Datum ponošenja zahtjeva za izvještajem: " + DatumPodnosenja.ToString() + Environment.NewLine);
            t.AppendText("********************************************************************************" + Environment.NewLine);
            foreach (String s in Podaci) {
                t.AppendText(s + Environment.NewLine);
            }

        }
        public override string ToString() {
            return Podnosioc.ToString() + " " + DatumPodnosenja.ToShortDateString();
        }
    }
}
