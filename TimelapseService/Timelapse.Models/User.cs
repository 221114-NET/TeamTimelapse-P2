using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Primary Address")]
        public string Address1 { get; set; }


        [Display(Name = "Secondary Address")]
        public string Address2 { get; set; }


        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        public User() { }
        public User(Guid id, string fname, string lname, string email, string password, string address1, string address2, int zipcode, string state, string country)
        {
            UserId = id;
            FirstName = fname;
            LastName = lname;
            EmailAddress = email;
            Password = password;
            Address1 = address1;
            Address2 = address2;
            ZipCode = zipcode;
            State = state;
            Country = country;
        }
    }
}