using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Timelapse.Models
{
    public class User
    {
        public User(int id, string fname, string lname, string email, string password, string address1, string address2, int zipcode, string state, string country)
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

        public User()
        {

        }


        public int UserId { get; set; }


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

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Primary Address")]
        [Required(ErrorMessage = "Primary Address is required")]
        public string  Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is required")]
        public int ZipCode { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required")]
        public string  Country { get; set; }

    }
}