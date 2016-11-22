using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class ArhivDAO : IDaoCrud<Arhiv> {
            public ArhivDAO() { }
            public long create(Arhiv entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into arhiv(idZatvorenici, ime, prezime, adresa, datum_rodjenja, idOdjeli, br_celije, datum_dolaska, datum_izlaska, napomene, slika, datum_arhiviranja, razlog) " +
                        "values(@ID, @Ime,@Prezime,@Adresa,@DatumR,@Odjel,@BrCelije,@DatumDol,@DatumIzl, @Napomene, @Slika, @DatumArh, @Razlog)", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Zatvorenik.Id;
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Zatvorenik.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Zatvorenik.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Zatvorenik.Adresa;
                    cmd.Parameters.Add("@DatumR", MySqlDbType.Date).Value = entity.Zatvorenik.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Odjel", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Zatvorenik.Blok) +1;
                    cmd.Parameters.Add("@BrCelije", MySqlDbType.Int32).Value = entity.Zatvorenik.BrojCelije;
                    cmd.Parameters.Add("@DatumDol", MySqlDbType.Date).Value = entity.Zatvorenik.DatumDolaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@DatumIzl", MySqlDbType.Date).Value = entity.Zatvorenik.DatumOdlaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Zatvorenik.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = dajByteOdSlike(entity.Zatvorenik.Slika);
                    cmd.Parameters.Add("@DatumArh", MySqlDbType.Date).Value = entity.DatumArhiviranja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Razlog", MySqlDbType.VarChar).Value = entity.RazlogArhiviranja;
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception) {
                    throw;
                }
            }

            public Arhiv read(Arhiv entity) {
                throw new NotImplementedException();
            }

            public Arhiv update(Arhiv entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update arhiv set ime = @Ime, prezime = @Prezime, adresa = @Adresa, datum_rodjenja = @DatumR, idOdjeli = @Odjel" +
                        "br_celije = @BrCelije, datum_dolaska = @DatumDol, datum_izlaska = @DatumIzl, napomene = @Napomene, slika = @Slika, where idZatvorenika = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.VarChar).Value = entity.Zatvorenik.Id;
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Zatvorenik.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Zatvorenik.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Zatvorenik.Adresa;
                    cmd.Parameters.Add("@DatumR", MySqlDbType.Date).Value = entity.Zatvorenik.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Odjel", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Zatvorenik.Blok) + 1;
                    cmd.Parameters.Add("@BrCelije", MySqlDbType.Int32).Value = entity.Zatvorenik.BrojCelije;
                    cmd.Parameters.Add("@DatumDol", MySqlDbType.Date).Value = entity.Zatvorenik.DatumDolaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@DatumIzl", MySqlDbType.Date).Value = entity.Zatvorenik.DatumOdlaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Zatvorenik.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = entity.Zatvorenik.Slika;
                    cmd.ExecuteNonQuery();
                    return entity;

                } catch (Exception) {

                    throw;
                }
            }

            public void delete(Arhiv entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("delete from arhiv where idZatvorenici = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Zatvorenik.Id;
                    cmd.ExecuteNonQuery();
                } catch (Exception) {
                    throw;
                }
            }

            public Arhiv getById(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from arhiv where idZatvorenici = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();                    
                    Zatvorenik z = new Zatvorenik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"),
                        (DateTime)r.GetDateTime("datum_rodjenja"), r.GetInt32("idZatvorenici"), Spol.Musko, r.GetString("napomene"),
                        (Blok)(r.GetInt32("idOdjeli") - 1), r.GetInt32("br_celije"), (DateTime)r.GetDateTime("datum_dolaska"), (DateTime)r.GetDateTime("datum_izlaska"), new Image());
                    return new Arhiv(z, (DateTime)r.GetDateTime("datum_arhiviranja"), r.GetString("razlog"));     
                } catch (Exception) {

                    throw;
                }
            }

            public List<Arhiv> dajUslovne() {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from arhiv where razlog = 'Uslovno'", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    List<Arhiv> a = new List<Arhiv>();
                    while (r.Read()) {
                        Image i = DAL.dajSlikuOdBajta(10, r);
                        Zatvorenik z = new Zatvorenik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"),
                        (DateTime)r.GetDateTime("datum_rodjenja"), r.GetInt32("idZatvorenici"), Spol.Musko, r.GetString("napomene"),
                        (Blok)(r.GetInt32("idOdjeli") - 1), r.GetInt32("br_celije"), (DateTime)r.GetDateTime("datum_dolaska"), (DateTime)r.GetDateTime("datum_izlaska"), i);
                        Arhiv arhiv = new Arhiv(z, (DateTime)r.GetDateTime("datum_arhiviranja"), r.GetString("razlog"));
                        a.Add(arhiv);
                    }
                    return a;
                } catch (Exception) {

                    throw;
                }
            }

            public List<Arhiv> getAll() {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from arhiv where razlog <> 'Uslovno'", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    List<Arhiv> a  =new List<Arhiv>();
                    while (r.Read()) {
                        Image i = DAL.dajSlikuOdBajta(10, r);
                        Zatvorenik z = new Zatvorenik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"),
                        (DateTime)r.GetDateTime("datum_rodjenja"), r.GetInt32("idZatvorenici"), Spol.Musko, r.GetString("napomene"),
                        (Blok)(r.GetInt32("idOdjeli") - 1), r.GetInt32("br_celije"), (DateTime)r.GetDateTime("datum_dolaska"), (DateTime)r.GetDateTime("datum_izlaska"), i);
                        Arhiv arhiv = new Arhiv(z, (DateTime)r.GetDateTime("datum_arhiviranja"), r.GetString("razlog"));
                        a.Add(arhiv);
                }
                    return a;
                } catch (Exception) {

                    throw;
                }
            }

            public List<Arhiv> getByExample(string name, string value) {
                throw new NotImplementedException();
            }
        }
    }
}
