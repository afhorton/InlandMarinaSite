using IMarinaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InlandMarina_MVC.Controllers
{
    [Authorize]
    public class MySlipsController : Controller
    {
        private readonly InlandMarinaContext _context;

        public MySlipsController(InlandMarinaContext context)
        {
            _context = context;
        }


        // GET: MySlipsController
        public ActionResult Index(string id = "All")
        {
            // filter rentals by current owner
            List<Lease> leases = null;
            // get the current owner id from the session state
            int? customerId = HttpContext.Session.GetInt32("CurrentCustomer");
            if (customerId == null)
            {
                customerId = 0; // return RedirectToAction("Login", "Account")
            }

            leases = LeaseManager.GetLeasesByCustomer((int)customerId);

            List<int> slipIds = leases.Select(l => l.SlipID).ToList(); // need to filter

            List<Slip> slips = new List<Slip>();
            if (slipIds.Count > 0)
            {
                int slipId = slipIds[0]; // choose the first slip id from the list
                Slip slip = SlipManager.GetSlipById(_context, slipId);
                slips.Add(slip);
            }
            return View(slips);

        }
    }
}
