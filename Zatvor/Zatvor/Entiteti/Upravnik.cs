using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor {
    public class Upravnik : Osoba {      

        public Upravnik(string _ime, string _prezime, string _adresa, DateTime _datumRodjenja, int _id, Spol _spol, string _napomene, DateTime _datumZaposlenja, List<Prisustvo> _prisustva, Image slika)
            : base(_ime, _prezime, _adresa, _datumRodjenja, _id, _spol, _napomene, slika) {
            DatumZaposlenja = _datumZaposlenja;
            Prisustva = _prisustva;           
        }

        public Upravnik() {

        }

        public Upravnik(string ime, string prezime): base(ime, prezime) { }

        public DateTime DatumZaposlenja { get; set; }
        public List<Prisustvo> Prisustva { get; set; }

        public void EvidentirajDolazak() {
            Prisustva.Add(new Prisustvo(base.Id, DateTime.Now, DateTime.Now));
        }

        public void EvidentirajIzlazak() {
            Prisustva[Prisustva.Count - 1].VrijemeIzlaska = DateTime.Now;
        }

        public override string ToString() {
            return "Upravnik: " + Ime + " " + Prezime;
        }

    }
}
