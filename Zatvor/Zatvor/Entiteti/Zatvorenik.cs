using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;


namespace Zatvor {
    public class Zatvorenik : Osoba {
        public Blok Blok { get; set; }
        public int BrojCelije { get; set; }
        public DateTime DatumDolaska { get; set; }
        public DateTime DatumOdlaska { get; set; }       
        public Zatvorenik() { }
        public Zatvorenik(string _ime, string _prezime, string _adresa, DateTime _datumRodjenja, int _id, Spol _spol, string _napomene, Blok _blok, int _brojCelije, DateTime _datumDolaska, DateTime _datumOdlaska, Image slika)
            : base(_ime, _prezime, _adresa, _datumRodjenja, _id, _spol, _napomene, slika) {
            Blok = _blok;
            BrojCelije = _brojCelije;
            DatumDolaska = _datumDolaska;
            DatumOdlaska = _datumOdlaska;
        }
        public Zatvorenik(string ime, string prezime) : base(ime, prezime) { }

        public string dajPodatkeOZatvoreniku() {
            return "Ime: " + Ime + Environment.NewLine +
                   "Prezime: " + Prezime + Environment.NewLine +
                   "Adresa: " + Adresa + Environment.NewLine +
                   "Datum rođenja: " + DatumRodjenja + Environment.NewLine +
                   "ID: " + Id + Environment.NewLine +
                   "Spol: " + Spol.ToString() + Environment.NewLine +
                   "Blok: " + Blok.ToString() + Environment.NewLine +
                   "Broj čelije: " + BrojCelije.ToString() + Environment.NewLine +
                   "Datum dolaska: " + DatumDolaska.ToShortDateString() + Environment.NewLine +
                   "Datum puštanja na slobodu: " + DatumOdlaska.ToShortDateString() + Environment.NewLine +
                   "Aktivnosti i napomene: " + Environment.NewLine + " -" + Napomene;
        }
        public override string ToString() {
            return "Zatvorenik: " + Ime + " " + Prezime;
        }

        
    }
}
