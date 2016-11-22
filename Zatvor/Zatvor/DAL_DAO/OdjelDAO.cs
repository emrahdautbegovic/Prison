using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor.DAL_DAO {
    public partial class DAL {
        public class OdjelDAO : IDaoCrud<Odjel> {
            public OdjelDAO() { }

            public long create(Odjel entity) { 
                try {
                    MySqlCommand cmd = new MySqlCommand("insert into odjeli(naziv, kapacitet) values(@Naziv, @Kapacitet, @Upravnik,)", con);
                    cmd.Parameters.Add("@Naziv", MySqlDbType.VarChar).Value = entity.Blok.ToString();
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                } catch (Exception) {
                    
                    throw;
                }

            }

            public Odjel read(Odjel entity) {
                throw new NotImplementedException();
            }

            public Odjel update(Odjel entity) {
                MySqlCommand cmd = new MySqlCommand("update odjeli set naziv='" + entity.Blok.ToString() + "' where id=" + entity.Id, con);
                cmd.ExecuteNonQuery();
                return entity;
            }

            public void delete(Odjel entity) {
                MySqlCommand cmd = new MySqlCommand("delete from odjeli where id=" + entity.Id, con);
                cmd.ExecuteNonQuery();
            }

            public Odjel getById(int id) {
                MySqlCommand cmd = new MySqlCommand("select * from odjeli where id=" + id, con);
                MySqlDataReader r = cmd.ExecuteReader();
                r.Read();
                return new Odjel((Blok)Enum.Parse(typeof(Blok), r.GetString(1), true));
            }

            public List<Odjel> getAll() {
                throw new NotImplementedException();
            }

            public List<Odjel> getByExample(string name, string value) {
                throw new NotImplementedException();
            }
        }
    }
}
