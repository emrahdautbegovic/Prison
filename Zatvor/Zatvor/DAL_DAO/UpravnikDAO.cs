using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class UpravnikDAO : IDaoCrud<Upravnik> {

            public UpravnikDAO() {

            }
            public long create(Upravnik entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into uposlenici(ime, prezime, adresa, posao, datum_rodjenja, spol, datum_zaposlenja, napomene, slika) " +
                        "values(@Ime, @Prezime, @Adresa, 'u', @DatumRodjenja, @Spol, @DatumZaposlenja, @Napomene, @Slika)", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumRodjenja", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Spol", MySqlDbType.VarChar).Value = entity.Spol;
                    cmd.Parameters.Add("@DatumZaposlenja", MySqlDbType.Date).Value = entity.DatumZaposlenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }


            }

            public Upravnik read(Upravnik entity) {
                throw new NotImplementedException();
            }

            public Upravnik update(Upravnik entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update uposlenici set ime = @Ime, prezime = @Prezime, adresa = @Adresa, posao = 'u', datum_rodjenja = @DatumRodjenja, spol = @Spol, datum_zaposlenja = @DatumZaposlenja, napomene = @Napomene, slika = @Slika where idUposlenici = @ID", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumRodjenja", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Spol", MySqlDbType.VarChar).Value = entity.Spol;
                    cmd.Parameters.Add("@DatumZaposlenja", MySqlDbType.Date).Value = entity.DatumZaposlenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return entity;
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public void delete(Upravnik entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("delete from uposlenici where idUposlenici = @ID ", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.ExecuteNonQuery();
                } catch (Exception) {

                    throw;
                }
            }

            public Upravnik getById(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where idUposlenici = @ID ", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    Upravnik u = null;
                    if (r.HasRows) {
                        Image i = DAL.dajSlikuOdBajta(11, r);
                        u = new Upravnik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"),
                            r.GetInt32("idUposlenici"), (Spol)Enum.Parse(typeof(Spol), r.GetString("spol"), true), r.GetString("napomene"), r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), i);
                    }
                    return u;

                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }
            public Upravnik dajUpravnika() {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where posao = 'u'", con);                    
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    Upravnik u = null;
                    if (r.HasRows) {
                        Image i = DAL.dajSlikuOdBajta(11, r);
                        u = new Upravnik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"),
                            r.GetInt32("idUposlenici"), (Spol)Enum.Parse(typeof(Spol), r.GetString("spol"), true), r.GetString("napomene"), r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), i);
                    }
                    return u;
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<Upravnik> getAll() {
                try {

                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where posao = 'u'", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    List<Upravnik> u = new List<Upravnik>();
                    while (r.Read()) {
                        if (r.HasRows) {
                            Image i = DAL.dajSlikuOdBajta(11, r);
                            u.Add(new Upravnik(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"),
                            r.GetInt32("idUposlenici"), (Spol)Enum.Parse(typeof(Spol), r.GetString("spol"), true), r.GetString("napomene"), r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), i));
                        }
                    }
                    return u;

                } catch (Exception) {

                    throw;
                }
            }

            public List<Upravnik> getByExample(string name, string value) {
                throw new NotImplementedException();
            }
        }
    }
}
