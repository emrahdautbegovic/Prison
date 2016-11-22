using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class ZatvorenikDAO : IDaoCrud<Zatvorenik> {
            public ZatvorenikDAO() { }

            public long create(Zatvorenik entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into zatvorenici(ime, prezime, adresa, datum_rodjenja, idOdjeli, br_celije, datum_dolaska, datum_izlaska, napomene, slika) " +
                        "values(@Ime,@Prezime,@Adresa,@DatumR,@Blok,@BrCelije,@DatumDol,@DatumIzl, @Napomene, @Slika)", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumR", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Blok", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Blok) + 1;
                    cmd.Parameters.Add("@BrCelije", MySqlDbType.Int32).Value = entity.BrojCelije;
                    cmd.Parameters.Add("@DatumDol", MySqlDbType.Date).Value = entity.DatumDolaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@DatumIzl", MySqlDbType.Date).Value = entity.DatumOdlaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception) {
                    throw;
                }
            }

            public Zatvorenik read(Zatvorenik entity) {
                throw new NotImplementedException();
            }

            public Zatvorenik update(Zatvorenik entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update zatvorenici set ime = @Ime, prezime = @Prezime, adresa = @Adresa, datum_rodjenja = @DatumR, " +
                        "idOdjeli = @Blok, br_celije = @BrCelije, datum_dolaska = @DatumDol, datum_izlaska = @DatumIzl, napomene = @Napomene, slika = @Slika where idZatvorenici = @ID", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumR", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@ZatvId", MySqlDbType.Int32).Value = entity.Id;
                    cmd.Parameters.Add("@Blok", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Blok) + 1;
                    cmd.Parameters.Add("@BrCelije", MySqlDbType.Int32).Value = entity.BrojCelije;
                    cmd.Parameters.Add("@DatumDol", MySqlDbType.Date).Value = entity.DatumDolaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@DatumIzl", MySqlDbType.Date).Value = entity.DatumOdlaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return entity;

                } catch (Exception) {

                    throw;
                }
            }

            public void delete(Zatvorenik entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("delete from zatvorenici where idZatvorenici = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.ExecuteNonQuery();
                } catch (Exception) {
                    throw;
                }
            }

            public Zatvorenik getById(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from zatvorenici where idZatvorenici = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    Image i = dajSlikuOdBajta(10, r);
                    
                    Zatvorenik z = new Zatvorenik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"),
                        r.GetDateTime("datum_rodjenja"), r.GetInt32("idZatvorenici"), Spol.Musko, r.GetString("napomene"),
                        (Blok)(r.GetInt32("idOdjeli") - 1), r.GetInt32("br_celije"), r.GetDateTime("datum_dolaska"), r.GetDateTime("datum_izlaska"), i);
                    return new Zatvorenik(); ;

                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Test");
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<Zatvorenik> getAll() {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from zatvorenici", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    List<Zatvorenik> z = new List<Zatvorenik>();
                    while (r.Read()) {
                        Image i = dajSlikuOdBajta(10, r);
                        z.Add(new Zatvorenik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"),
                        r.GetInt32("idZatvorenici"), Spol.Musko, r.GetString("napomene"),
                        (Blok)(r.GetInt32("idOdjeli") - 1), r.GetInt32("br_celije"), r.GetDateTime("datum_dolaska"), r.GetDateTime("datum_izlaska"), i));
                    }
                    return z;
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<Zatvorenik> getByExample(string name, string value) {
                throw new NotImplementedException();
            }
        }
    }
}
