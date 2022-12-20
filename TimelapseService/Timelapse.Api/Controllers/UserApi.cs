using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timelapse.Logic;
using Timelapse.Models;

namespace Timelapse.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserApi : ControllerBase
    {
        private readonly IUserLogic _ibus;

        public UserApi (IUserLogic ibus)
        {
            _ibus = ibus;
        }


        [HttpPost("UserAccount")]
        public ActionResult<User> PostAccount(User r)
        {

            User r1 = this._ibus.PostAccount(r);
            return r1;

       
        }
    }
}