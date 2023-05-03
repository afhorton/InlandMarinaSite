using IMarinaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InlandMarina_MVC.Controllers
{
    [Authorize]
    public class LeaseController : Controller
    {
        private InlandMarinaContext context;

        public LeaseController(InlandMarinaContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index(string id = "All")
        {
            var docks = context.Docks
                .OrderBy(d => d.ID)
                .ToList();

            List<Slip> slips;
            if (id == "All")
            {
                slips = context.Slips
                    .OrderBy(s => s.ID)
                    .ToList();
            }
            else
            {
                slips = context.Slips
                    .Where(p => p.Dock.Name == id)
                    .OrderBy(p => p.ID)
                    .ToList();
            }

            // Get the IDs of leased slips
            var leasedSlipIds = context.Leases.Select(l => l.SlipID).ToList();

            // Filter out the slips that have at least one lease
            List<Slip> availableSlips = slips.Where(slip => !leasedSlipIds.Contains(slip.ID)).ToList();

            // use ViewBag to pass data to view
            ViewBag.Docks = docks;
            ViewBag.SelectedCategoryName = id;

            // bind products to view
            return View(availableSlips);
        }

       
        // this ActionResult gets a slip to lease

        public ActionResult LeaseSlip(int id) {
            try
            {
                Slip slip = SlipManager.GetSlipById(context, id);
                return View(slip);
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(null);
            }
        }



        // this post method creates a new lease

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLease(int slipId)
        {
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                // Check if the slip is already leased
                if (db.Leases.Any(l => l.SlipID == slipId))
                {
                    TempData["Message"] = "Slip is already leased.";
                    return RedirectToAction("Index");
                }

                // Get the customer ID from the cookie
                int customerId;
                if (!int.TryParse(HttpContext.Session.GetInt32("CurrentCustomer").ToString(), out customerId))
                {
                    TempData["Message"] = "Invalid customer ID.";
                    return RedirectToAction("Index");
                }

                // Retrieve the slip entity from the database
                Slip slip = db.Slips.Find(slipId);
                if (slip == null)
                {
                    TempData["Message"] = "Invalid slip ID.";
                    return RedirectToAction("Index");
                }

                // Create a new lease with the retrieved slip entity and the customer ID
                Lease newLease = new Lease
                {
                    Slip = slip,
                    CustomerID = customerId
                };
                db.Leases.Add(newLease);
                db.SaveChanges();
            }

            TempData["Message"] = "Leased slip successfully.";
            return RedirectToAction("Index");
        }




    }
}
