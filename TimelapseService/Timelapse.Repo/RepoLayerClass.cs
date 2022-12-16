using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Timelapse.Models;

namespace Timelapse.Repo
{

    public interface IRepoLayerClass
    {
        public Register PostAccount(Register e);
    }
    public class RepoLayerClass : IRepoLayerClass
    {
        
         public Register PostAccount(Register r)
        {
            
            if (File.Exists("CustomerList.json"))
            {
                string oldList = File.ReadAllText("CustomerList.json");
                List<Register> customerList = JsonSerializer.Deserialize<List<Register>>(oldList)!;

                customerList.Add(r);

                string CustObjectsToJString = JsonSerializer.Serialize(customerList);
                File.WriteAllText("CustomerList.json", CustObjectsToJString); 
                return r;

            }
            else
            {
                List<Register> customerList = new List<Register>();
                customerList.Add(r);

                string CustObjectsToJString = JsonSerializer.Serialize(customerList);
                File.WriteAllText("CustomerList.json", CustObjectsToJString);
                return r;

            }
        }
    }
}