using IMarinaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InlandMarina_MVC.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly InlandMarinaContext _context;

        public RegistrationController(InlandMarinaContext context)
        {
            _context = context;
        }

        // GET: RegistrationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RegistrationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegistrationController/Create
        public ActionResult Create()
        {
            return View(new Customer());
        }

        // POST: RegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer newCustomer)
        {
            newCustomer.Leases = null; 
            if (ModelState.IsValid)
            {

                try
                {

                    CustomerManager.AddCustomer(newCustomer);
                    TempData["Message"] = $"Successfully added customer {newCustomer.Username}";
                    // do not set TempData["IsError"]
                    return RedirectToAction("Login", "Account");

                }
                catch
                {
                    TempData["Message"] = "Database connection error. Try again later.";
                    TempData["IsError"] = true;
                    return View(newCustomer);
                }
            }
            else // validation errors
            {
                return View(newCustomer);
            }           
           
        }

    }
}
