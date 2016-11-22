using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class AdminDAO : IDaoCrud<Admin> {
            public long create(Admin entity) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("insert into administratori(ime, prezime, user, password, mail, tip, slika) values(@Ime, @Prezime, @User, @Pass, @Mail, @Tip, @Slika)", con)) {
                        cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                        cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                        cmd.Parameters.Add("@User", MySqlDbType.VarChar).Value = entity.UserName;
                        cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = entity.Password.ToString();
                        cmd.Parameters.Add("@Mail", MySqlDbType.VarChar).Value = entity.Mail;
                        cmd.Parameters.Add("@Tip", MySqlDbType.Int32).Value = Convert.ToInt32(entity.Tip);
                        cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = dajByteOdSlike(entity.Slika);
                        cmd.ExecuteNonQuery();
                        return cmd.LastInsertedId;
                    }
                } catch (Exception) {
                    
                    throw;
                }
            }

            public Admin read(Admin entity) {
                throw new NotImplementedException();
            }

            public Admin update(Admin entity) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("update administratori set ime = @Ime, prezime = @Prezime, user = @User, password = @Pass, mail = @Mail, slika = @Slika where id = @ID", con)) {
                        cmd.Parameters.Add("@Ime", MySqlDbType.VarChar).Value = entity.Ime;
                        cmd.Parameters.Add("@Prezime", MySqlDbType.VarChar).Value = entity.Prezime;
                        cmd.Parameters.Add("@User", MySqlDbType.VarChar).Value = entity.UserName;
                        cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = entity.Password.ToString();
                        cmd.Parameters.Add("@Mail", MySqlDbType.VarChar).Value = entity.Mail;                        
                        cmd.Parameters.Add("@Slika", MySqlDbType.MediumBlob).Value = dajByteOdSlike(entity.Slika);
                        cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                        cmd.ExecuteNonQuery();
                        return entity;
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public Admin dajAdmina(string user, string pass) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from administratori where user = @User and password = @Pass", con);
                    cmd.Parameters.Add("@User", MySqlDbType.VarChar).Value = user;
                    cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = pass;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    if (r.HasRows) {
                        Image i = dajSlikuOdBajta(7, r);
                        Admin a = new Admin(r.GetInt32("id"), r.GetString("ime"), r.GetString("prezime"), r.GetString("user"), r.GetString("password"), r.GetString("mail"), (TipAdmina)r.GetInt32("tip"), i);
                        r.Close();
                        return a;
                    } else return null;
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public void delete(Admin entity) {
                try {
                    MySqlCommand cmd = new MySqlCommand("delete from administratori where id = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = entity.Id;
                    cmd.ExecuteNonQuery();
                } catch (Exception ex) {

                    System.Windows.Forms.MessageBox.Show(ex.Message, "Problem kod brisanja admina!");
                }
            }

            public Admin getById(int id) {
                try {
                    MySqlCommand cmd = new MySqlCommand("select * from administratori where id = @ID", con);
                    cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    Image i = dajSlikuOdBajta(7,r);
                    return new Admin(r.GetInt32("id"), r.GetString("ime"), r.GetString("prezime"), r.GetString("user"), r.GetString("password"), r.GetString("mail"), (TipAdmina)r.GetInt32("tip"), i);
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<Admin> getAll() {
               try {
                    MySqlCommand cmd = new MySqlCommand("select * from administratori", con);                    
                    MySqlDataReader r = cmd.ExecuteReader();
                    if (r.HasRows) {
                        List<Admin> admini = new List<Admin>();
                        while (r.Read()) {
                            Image i = dajSlikuOdBajta(7, r);
                            admini.Add(new Admin(r.GetInt32("id"), r.GetString("ime"), r.GetString("prezime"), r.GetString("user"), r.GetString("password"), r.GetString("mail"), (TipAdmina)r.GetInt32("tip"), i));
                        }
                        return admini;
                    } else return null;            
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message);
                    throw;
                }
            }

            public List<Admin> getByExample(string name, string value) {
                throw new NotImplementedException();
            }

            public string dajAdminovMail() {
                try {
                        MySqlCommand cmd = new MySqlCommand("select mail from administratori where tip = 0", con);
                        MySqlDataReader r = cmd.ExecuteReader();
                        r.Read();
                        string s;
                        if (r.HasRows) { s = r.GetString(0); r.Close(); return s; } 
                        else { r.Close(); return string.Empty; }                                    
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message ,"Problem u čitanju iz baze");
                    throw;
                } 
            }

            public string dajAdminovPass() {
                try {
                    MySqlCommand cmd = new MySqlCommand("select password from administratori where tip = 0", con);
                    MySqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    string s;
                    if (r.HasRows) { s = r.GetString(0); r.Close(); return s; } else { r.Close(); return string.Empty; }  
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(ex.Message, "Problem u čitanju iz baze");
                    throw;
                }
            }
            public Admin dajPoMailu(string mail) {
                try {
                    using (MySqlCommand cmd = new MySqlCommand("select * from administratori where mail = @Mail", con)) {
                        cmd.Parameters.Add("@Mail", MySqlDbType.VarChar).Value = mail;
                        MySqlDataReader r = cmd.ExecuteReader();
                        if (r.HasRows) {
                            r.Read();
                            Image i = dajSlikuOdBajta(7, r);
                            return new Admin(r.GetInt32("id"), r.GetString("ime"), r.GetString("prezime"), r.GetString("user"),
                                r.GetString("password"), r.GetString("mail"), (TipAdmina)r.GetInt32("tip"), i);
                        } else throw new Exception("Nepostojeći administrator!");
                    }

                } catch (Exception) {
                    
                    throw;
                }
            } 
        }
    }
}
