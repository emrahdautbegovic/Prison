using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class UserDAO : IDaoCrud<User>{
            public long create(User entity) {
                try {
                    using(MySqlCommand cmd = new MySqlCommand("insert into korisnici values(@ID, @User, @Pass, @Mail)", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                        cmd.Parameters.Add("@User", MySqlDbType.VarChar).Value = entity.UserName;
                        cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = entity.Password;
                        cmd.Parameters.Add("@Mail", MySqlDbType.VarChar).Value = entity.Mail;
                        cmd.ExecuteNonQuery();
                        return cmd.LastInsertedId;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public User read(User entity) {
                throw new NotImplementedException();
            }

            public User update(User entity) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("update korisnici set idUposlenici = @ID, user = @User, password = @Pass, mail = @Mail)", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                        cmd.Parameters.Add("@User", MySqlDbType.VarChar).Value = entity.UserName;
                        cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = entity.Password;
                        cmd.Parameters.Add("@Mail", MySqlDbType.VarChar).Value = entity.Mail;
                        cmd.ExecuteNonQuery();
                        return entity;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public void delete(User entity) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("delete from korisnici where idUposlenici = @ID", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                        cmd.ExecuteNonQuery();                        
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public User getById(int id) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from korisnici where idUposlenici = @ID", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        User u = new User(r.GetInt32("idUposlenici"), r.GetString("user"), r.GetString("password"), r.GetString("mail"));
                        r.Close();
                        return u;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }


            public User dajUsera(string user, string pass) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from korisnici where user = @User and password = @Pass", con)) {
                        cmd.Parameters.Add("@User", MySqlDbType.VarChar).Value = user;
                        cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = pass;
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        if (r.HasRows) {
                            User u = new User(r.GetInt32("idUposlenici"), r.GetString("user"), r.GetString("password"), r.GetString("mail"));
                            r.Close();
                            return u;
                        } else { r.Close(); return null; }                                           
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Problem kod verifikacije korisnika");
                    throw;
                }
            }

            public List<User> getAll() {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from korisnici", con)) {                        
                        MySqlDataReader r = cmd.ExecuteReader();
                        List<User> l = new List<User>();
                        while (r.Read()) l.Add(new User(r.GetInt32("idUposlenici"), r.GetString("user"), r.GetString("password"), r.GetString("mail")));
                        r.Close();
                        return l;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<User> getByExample(string name, string value) {
                throw new NotImplementedException();
            }

            public User dajPoMailu(string p) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from korisnici where mail = @Mail", con)) {
                        cmd.Parameters.Add("@Mail", MySqlDbType.VarChar).Value = p;
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        User u = new User(r.GetInt32("idUposlenici"), r.GetString("user"), r.GetString("password"), r.GetString("mail"));
                        r.Close();
                        return u;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public Image dajSlikuUseraPoId(int id) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select slika from uposlenici where idUposlenici = @ID", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        Image i = dajSlikuOdBajta(0, r);
                        r.Close();
                        return i;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public string dajImeUseraPoId(int id) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select ime from uposlenici where idUposlenici = @ID", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        string s = r.GetString(0);
                        r.Close();
                        return s;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public string dajPrezimeUseraPoId(int id) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select prezime from uposlenici where idUposlenici = @ID", con)) {
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        string s = r.GetString(0);
                        r.Close();
                        return s;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
    }
}
