using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class PrisustvoDAO : IDaoCrud<Prisustvo> {
            public PrisustvoDAO() { }
            public long create(Prisustvo entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into prisustva(idUposlenici, vrijeme_dolaska, vrijeme_odlaska) " +
                "values(@ID, @DatDolaska, @DatOdlaska)", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.Parameters.Add("@DatDolaska", MySqlDbType.DateTime).Value = entity.VrijemeDolaska.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.Parameters.Add("@DatOdlaska", MySqlDbType.Date).Value = null;
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception) {

                    throw;
                }
            }

            public Prisustvo read(Prisustvo entity) {
                throw new NotImplementedException();
            }

            public Prisustvo update(Prisustvo entity) {
                throw new NotImplementedException();
            }

            public void delete(Prisustvo entity) {
                throw new NotImplementedException();
            }

            public Prisustvo getById(int id) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from prisustva where idUposlenici = @ID and vrijeme_dolaska is not null and vrijeme_odlaska is not null", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                        using (MySqlDataReader r = cmd.ExecuteReader()) {
                            if (r.HasRows) {
                                r.Read();
                                return new Prisustvo(r.GetInt32("idUposlenici"), r.GetDateTime("vrijeme_dolaska"), r.GetDateTime("vrijeme_odlaska"));
                            } else throw new Exception("Nepostojeći uposlenik");
                        }
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Greška pri čitanju iz baze!");
                    throw;
                } 
            }

            public List<Prisustvo> getAll() {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from prisustva where vrijeme_dolaska is not null and vrijeme_odlaska is not null", con)) {
                        using (MySqlDataReader r = cmd.ExecuteReader()) {
                            List<Prisustvo> p = new List<Prisustvo>();                           
                                while (r.Read()) p.Add(new Prisustvo(r.GetInt32("idUposlenici"), r.GetDateTime("vrijeme_dolaska"), r.GetDateTime("vrijeme_odlaska")));
                                return p;                            
                        }
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Greška pri čitanju prisustava iz baze!");
                    throw;
                } 
            }

            public List<Prisustvo> dajTrenutne() {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from prisustva where vrijeme_dolaska is not null and vrijeme_odlaska is null", con)) {
                        using (MySqlDataReader r = cmd.ExecuteReader()) {
                            List<Prisustvo> p = new List<Prisustvo>();
                            while (r.Read()) p.Add(new Prisustvo(r.GetInt32("idUposlenici"), r.GetDateTime("vrijeme_dolaska"), r.GetDateTime("vrijeme_odlaska")));
                            return p;
                        }
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Greška pri čitanju prisustava iz baze!");
                    throw;
                } 
            }

            public List<Prisustvo> getByExample(string name, string value) {
                throw new NotImplementedException();
            }

            public void unesiDolazak(int id, DateTime time) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update prisustva set vrijeme_dolaska = @Time where idUposlenici = @ID and vrijeme_odlaska is null", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    cmd.Parameters.Add("@Time", MySqlDbType.DateTime).Value = time.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.ExecuteNonQuery();
                } catch (Exception) {

                    throw;
                }
            }

            public bool jeLiDosao(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select count(*) from prisustva where idUposlenici = @ID and vrijeme_dolaska is not null and vrijeme_odlaska is null", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    if (r.GetInt32(0) == 1) { r.Close(); return true; } 
                    else { r.Close(); return false; }

                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Problem kod provjere da li odlazi il dolazi!");
                    throw;
                }
            }
            public void unesiOdlazak(int id, DateTime time) {
                try {
                    MySqlCommand cmd = new MySqlCommand("update prisustva set vrijeme_odlaska = @Time where idUposlenici = @ID and vrijeme_odlaska is null", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    cmd.Parameters.Add("@Time", MySqlDbType.DateTime).Value = time.ToString("yyyy-MM-dd H:mm:ss");
                    cmd.ExecuteNonQuery();
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message, "Problem kod unošenja dolaska na posao");
                    throw;
                }
            }

            public void deleteAll() {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("delete from prisustva", con)) {
                        cmd.ExecuteNonQuery();
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Greška pri brisanju prisustava iz baze!");
                    throw;
                } 
            }
        }
    }
}
