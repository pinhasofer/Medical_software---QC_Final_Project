using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Doctor {
        private String username, password, id;

        //getters and setters
        /*public string Username { get; set; }
        public string Password{ get; set; }
        public string Id { get; set; }*/

        public string Username {
            get { return username; }
            set { username = value; }
        }
        public string Password { 
            get { return password; }
            set { password = value; }
        }
        public string Id {
            get { return id;}
            set { id = value; }
        }

        //Ctors
        public Doctor(string username, string password, string id)
        {
            this.username = username;
            this.password = password;
            this.id = id;
        }

        public Doctor(Doctor other)
        {
            this.username = other.username;
            this.password = other.password;
            this.id       = other.id;
        }
    }
}
