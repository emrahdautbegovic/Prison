using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class CuvarDAO : IDaoCrud<Cuvar> {
            public CuvarDAO() {

            }
            public long create(Cuvar entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into uposlenici(ime, prezime, adresa, posao, datum_rodjenja, idOdjeli, spol, datum_zaposlenja, zaduzenja, napomene, slika)"+
                        "values (@Ime,@Prezime,@Adresa,'c',@DatumRodjenja, @Blok, @Spol, @DatumZaposlenja, @Zaduzenja, @Napomene, @Slika)", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumRodjenja", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Blok", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Blok) + 1;
                    cmd.Parameters.Add("@Spol", MySqlDbType.VarChar).Value = entity.Spol;
                    cmd.Parameters.Add("@DatumZaposlenja", MySqlDbType.Date).Value = entity.DatumZaposlenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Zaduzenja", MySqlDbType.VarChar).Value = entity.Zaduzenje;
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception) {
                    
                    throw;
                }
            }

            public Cuvar read(Cuvar entity) {
                throw new NotImplementedException();
            }

            public Cuvar update(Cuvar entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update uposlenici set ime = @Ime, prezime = @Prezime, adresa = @Adresa, posao = 'c', datum_rodjenja = @DatumRodjenja, idOdjeli = @Blok, spol = @Spol, datum_zaposlenja = @DatumZaposlenja, zaduzenja = @Zaduzenja, napomene = @Napomene, slika = @Slika where idUposlenici = @ID ", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumRodjenja", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Blok", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Blok) + 1;
                    cmd.Parameters.Add("@Spol", MySqlDbType.VarChar).Value = entity.Spol;
                    cmd.Parameters.Add("@DatumZaposlenja", MySqlDbType.Date).Value = entity.DatumZaposlenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Zaduzenja", MySqlDbType.VarChar).Value = entity.Zaduzenje;
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return entity;

                } catch (Exception) {

                    throw;
                }
            }

            public void delete(Cuvar entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("delete from uposlenici where idUposlenici = @ID ", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.ExecuteNonQuery();


                } catch (Exception) {

                    throw;
                }
            }

            public Cuvar getById(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where idUposlenici = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    Cuvar c = null;
                    if (r.HasRows) {
                        Image i = DAL.dajSlikuOdBajta(11, r);
                        c = new Cuvar(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"), r.GetInt32("idUposlenici"),
                            (Spol)Enum.Parse(typeof(Spol), r.GetString("spol"), true), r.GetString("napomene"), (Blok)(r.GetInt32("idOdjeli") - 1),
                            r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), r.GetString("zaduzenja"), i);
                    } return c;
                } catch (Exception) {

                    throw;
                }
            }

            public List<Cuvar> getAll() {
                try {

                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where posao = 'c'", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    List<Cuvar> c = new List<Cuvar>();
                    while (r.Read()) {
                        if (r.HasRows) {
                            Image i = DAL.dajSlikuOdBajta(11, r);
                            c.Add(new Cuvar(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"),
                                r.GetInt32("idUposlenici"), (Spol)Enum.Parse(typeof(Spol), r.GetString("spol"), true), r.GetString("napomene"), (Blok)(r.GetInt32("idOdjeli") - 1),
                                 r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), r.GetString("zaduzenja"), i));
                        }
                    }
                    return c;

                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<Cuvar> getByExample(string name, string value) {
                throw new NotImplementedException();
            }
        }
    }
}
