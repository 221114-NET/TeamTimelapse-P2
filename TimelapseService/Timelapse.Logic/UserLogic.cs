using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelapse.Models;
using Timelapse.Repo;


namespace Timelapse.Logic
{
    public interface IUserLogic
    {
        public User PostAccount(User r);
    }
    public class UserLogic : IUserLogic
    {
        
        private readonly IUserRepo _repo;

        public UserLogic(IUserRepo repo)
        {
            _repo = repo;
        }

        public User PostAccount(User r)
        {
            User r1 = this._repo.PostAccount(r);
            return r1;
        }

    }
}