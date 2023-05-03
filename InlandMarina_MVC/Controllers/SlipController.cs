using IMarinaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace InlandMarina_MVC.Controllers
{
    public class SlipController : Controller
    {
        private InlandMarinaContext context;

        public SlipController(InlandMarinaContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List", "Slip");
        }

        [Route("[controller]s/{id?}")]
        public IActionResult List(string id = "All")
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

    }
}
