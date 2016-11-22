using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor.DAL_DAO {
    partial class DAL {
        public class DAOFactory             // Inner klasa 
        {
            // Method factory dizajn pattern
            // public enum FactoryTip { MySQL }
            // public static DAOFactory GetDAOFactory(FactoryTip tip){
            //    switch (tip)
            //    {
            //        case FactoryTip.MySQL:
            //            return new MySQLDAOFactory();
            //    }
            //}

            private static DAOFactory instanca = null;
            public static DAOFactory Instanca {
                get { return (instanca == null) ? instanca = new DAOFactory() : instanca; }
            }

            private DAOFactory() { }

            public ZatvorenikDAO getZatvorenikDAO() {
                return new ZatvorenikDAO();
            }
            public CuvarDAO getCuvarDAO() {
                return new CuvarDAO();
            }
            public OdjelDAO getOdjelDAO() {
                return new OdjelDAO();
            }
            public PrisustvoDAO getPrisustvoDAO() {
                return new PrisustvoDAO();
            }
            public UpravnikDAO getUpravnikDAO() {
                return new UpravnikDAO();
            }
            public UpravnikOdjelaDAO getUpravnikOdjelaDAO() {
                return new UpravnikOdjelaDAO();
            }
            public UserDAO getUserDAO() {
                return new UserDAO();
            }
            public AdminDAO getAdminDAO() {
                return new AdminDAO();
            }
            public ArhivDAO getArhivDAO() {
                return new ArhivDAO();
            }
        }
    }
}
