using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Timelapse.Models;

namespace Timelapse.Repo
{

    public interface IUserRepo
    {
         User PostAccount(User r);
    }
    public class UserRepo : IUserRepo 
    {

        public User PostAccount(User r)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand command = new SqlCommand("INSERT INTO User (FirstName,LastName, EmailAddress, Password, ConfirmPassword) VALUES (@FirstName,@LastName, @EmailAddress,@Password);", conn);
            conn.Open();
            command.Parameters.AddWithValue("@FirstName", r.FirstName);
            command.Parameters.AddWithValue("@LastName", r.LastName);
            command.Parameters.AddWithValue("@EmailAddress", r.EmailAddress);
            command.Parameters.AddWithValue("@Password", r.Password);
            

            int rowsCreated = command.ExecuteNonQuery();

            if (rowsCreated == 1)
            {
                conn.Close();
                return r;

            }
            else
            {
                return null;
            }
        }
    }
}
