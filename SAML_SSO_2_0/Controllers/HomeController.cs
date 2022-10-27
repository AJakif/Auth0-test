using ITfoxtec.Identity.Saml2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAML_SSO_2_0.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SAML_SSO_2_0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            TestModel test = new TestModel();
            test.name = "bill";
            test.age = 12;
            TempData["model"] = JsonConvert.SerializeObject(test);
            //return View();
            return RedirectToAction("Privacy");
        }

        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            TestModel test = new TestModel();

            test = JsonConvert.DeserializeObject<TestModel>(TempData["model"].ToString());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
