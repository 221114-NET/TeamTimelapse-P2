using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class Register
    {
         public Register(int id, string fname, string lname,string email, string password)
        {
            RegisterId = id;
            FirstName = fname;
            LastName = lname;
            EmailAddress = email;
            Password = password;
        }

        public Register()
        {
            
        }

        
        public int RegisterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
 
    }
}