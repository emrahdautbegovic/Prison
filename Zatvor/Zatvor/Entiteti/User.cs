using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatvor {
    public class User {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public User() { }
        public User(int id, string user, string pass, string mail) {
            Id = id;
            UserName = user;
            Password = pass;
            Mail = mail;
        }

        public override string ToString() {
            return UserName;
        }
    }
}
