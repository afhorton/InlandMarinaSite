using InlandMarina_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InlandMarina_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

    }
}