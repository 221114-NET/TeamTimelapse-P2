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
    [Route("api/[controller]")]
    public class Timelapse : ControllerBase
    {
        private readonly IBusinessLayerClass _ibus;

        public Timelapse (IBusinessLayerClass ibus)
        {
            _ibus = ibus;
        }


        [HttpPost("RegisterAccount")]
        public ActionResult<Register> PostAccount(Register r)
        {

            Register r1 = this._ibus.PostAccount(r);
            return r1;

       
        }
    }
}