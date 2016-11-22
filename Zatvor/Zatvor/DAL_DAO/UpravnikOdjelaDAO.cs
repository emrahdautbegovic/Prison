using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class UpravnikOdjelaDAO : IDaoCrud<UpravnikOdjela> {
            public UpravnikOdjelaDAO() { }


            public long create(UpravnikOdjela entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into uposlenici(ime, prezime, adresa, posao, datum_rodjenja, idOdjeli, spol, datum_zaposlenja, napomene, slika) " +
                        " values(@Ime, @Prezime, @Adresa, 'uo', @DatumRodjenja, @Blok, @Spol, @DatumZaposlenja, @Napomene, @Slika)", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumRodjenja", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Blok", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Blok) + 1;
                    cmd.Parameters.Add("@Spol", MySqlDbType.VarChar).Value = entity.Spol;
                    cmd.Parameters.Add("@DatumZaposlenja", MySqlDbType.Date).Value = entity.DatumZaposlenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception) {

                    throw;
                }
            }

            public UpravnikOdjela read(UpravnikOdjela entity) {
                throw new NotImplementedException();
            }

            public UpravnikOdjela update(UpravnikOdjela entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update uposlenici set ime = @Ime, prezime = @Prezime, posao = 'uo', adresa = @Adresa, datum_rodjenja = @DatumRodjenja, idOdjeli = @Blok, spol = @Spol, datum_zaposlenja = @DatumZaposlenja, napomene = @Napomene, slika = @Slika where idUposlenici = @ID", con);
                    cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                    cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                    cmd.Parameters.Add("@Adresa", MySqlDbType.VarChar).Value = entity.Adresa;
                    cmd.Parameters.Add("@DatumRodjenja", MySqlDbType.Date).Value = entity.DatumRodjenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Blok", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Blok) + 1;
                    cmd.Parameters.Add("@Spol", MySqlDbType.VarChar).Value = entity.Spol;
                    cmd.Parameters.Add("@DatumZaposlenja", MySqlDbType.Date).Value = entity.DatumZaposlenja.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@Napomene", MySqlDbType.VarChar).Value = entity.Napomene;
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = DAL.dajByteOdSlike(entity.Slika);
                    cmd.ExecuteNonQuery();
                    return entity;

                } catch (Exception) {

                    throw;
                }
            }

            public void delete(UpravnikOdjela entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("delete from uposlenici where idUposlenici = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.ExecuteNonQuery();
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message);                    
                }
            }

            public UpravnikOdjela getById(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where idUposlenici = @ID ", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    UpravnikOdjela uo = null;
                    if (r.HasRows) {
                        Image i = DAL.dajSlikuOdBajta(11, r);
                        uo = new UpravnikOdjela(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"),
                            r.GetInt32("idUposlenici"), (Spol)Enum.Parse(typeof(Spol), r.GetString("spol"), true), r.GetString("napomene"), r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), (Blok)(r.GetInt32("idOdjeli") - 1), i);
                    } 
                    return uo;

                } catch (Exception) {

                    throw;
                }
            }

            public List<UpravnikOdjela> getAll() {
                try {

                    MySqlCommand cmd = new MySqlCommand("select * from uposlenici where posao = 'uo'", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    List<UpravnikOdjela> uo = new List<UpravnikOdjela>();
                    while (r.Read()) {
                        if (r.HasRows) {
                            Image i = DAL.dajSlikuOdBajta(11, r);
                            uo.Add(new UpravnikOdjela(r.GetString("ime"), r.GetString("prezime"), r.GetString("adresa"), r.GetDateTime("datum_rodjenja"), r.GetInt32("idUposlenici"), (Spol)Enum.Parse(typeof(Spol),
                            r.GetString("spol"), true), r.GetString("napomene"), r.GetDateTime("datum_zaposlenja"), new List<Prisustvo>(), (Blok)(r.GetInt32("idOdjeli") - 1), i));
                        }
                    }
                    return uo;

                } catch (Exception) {

                    throw;
                } 
            }

            public List<UpravnikOdjela> getByExample(string name, string value) {
                throw new NotImplementedException();
            }
        }
    }
}
