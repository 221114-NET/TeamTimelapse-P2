using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelapse.Models;
using Timelapse.Repo;

namespace Timelapse.Logic
{
     public interface IBusinessLayerClass
    {
        public Register PostAccount(Register r);
    }
    public class BusinessLayerClass : IBusinessLayerClass
    {
        
        private readonly IRepoLayerClass _repo;

        public BusinessLayerClass(IRepoLayerClass repo)
        {
            _repo = repo;
        }

        public Register PostAccount(Register r)
        {
            Register r1 = this._repo.PostAccount(r);
            return r1;
        }

    }
}