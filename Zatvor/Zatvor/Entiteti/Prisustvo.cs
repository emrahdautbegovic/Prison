using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor {
    public class Prisustvo {
        public int Id { get; set; }
        public Prisustvo(int id, DateTime _vrijemeDolaska, DateTime _vrijemeIzlaska) {
            VrijemeDolaska = _vrijemeDolaska;
            VrijemeIzlaska = _vrijemeIzlaska;
            Id = id;
        }
        public Prisustvo() { }

        public DateTime VrijemeDolaska { get; set; }
        public DateTime VrijemeIzlaska { get; set; }

        public TimeSpan DajProtekloVrijeme() {
            return VrijemeIzlaska.Subtract(VrijemeDolaska);

        }
        public override string ToString() {
            return "ID>> " + Id.ToString() + ";  Dolazak>> " + VrijemeDolaska.ToString() + ";  Odlazak>> " + VrijemeIzlaska.ToString()
                + ";  Radno vrijeme>> " + (DajProtekloVrijeme().Minutes / 60).ToString() + "h " + (DajProtekloVrijeme().Minutes % 60).ToString() + "m";
        }
    }
}
