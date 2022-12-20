using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Timelapse.Models;

namespace Timelapse.UI.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            //we need to call our api to add data in database
            using ( var client=new HttpClient())
            {
               client.BaseAddress=new Uri("https://localhost:7167");
              var json= JsonConvert.SerializeObject(user);
              var content=new StringContent(json);
              var response=client.PostAsync("/UserApi/UserAccount",content);
               if(response.IsCompletedSuccessfully)
               {
       
               };
               
            }


            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
