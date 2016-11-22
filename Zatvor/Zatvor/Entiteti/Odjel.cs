using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor {
    public class Odjel{

        public int Id { get; set; }
        public Blok Blok { get; set; }
        public int BrojMjesta { get; set; }
        public List<Cuvar> Cuvari { get; set; }
        public UpravnikOdjela Upravnik { get; set; }
        public List<Zatvorenik> Zatvorenici { get; set; }
        public Odjel() { }
        public Odjel(Blok blok) { Blok = blok; }
        public Odjel(Blok _blok, int _brojMjesta, List<Cuvar> _cuvari, UpravnikOdjela _upravnik, List<Zatvorenik> _zatvorenici) {
            Blok = _blok;
            BrojMjesta = _brojMjesta;
            Cuvari = _cuvari;
            Upravnik = _upravnik;
            Zatvorenici = _zatvorenici;
        }

        public override string ToString() {
            return "Blok A";
        }
    }

}
