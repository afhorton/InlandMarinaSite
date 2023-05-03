using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMarinaData
{
    public class SlipManager
    {
        /// <summary>
        /// retrieve all slips
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of slips or null if none</returns>
        public static List<Slip> GetSlips(InlandMarinaContext db)
        {
            List<Slip> slips = null;

            slips = db.Slips.Include(s => s.Dock).ToList();

            return slips;

        }

        /// <summary>
        /// filter the leases based on their docks
        /// </summary>
        /// <param name="db">InlandMarinaContext</param>
        /// <param name="dockId">the id of the dock</param>
        /// <returns>a list of leases filtered by dock</returns>
        public static List<Slip> GetSlipsByDock(InlandMarinaContext db, int dockId)
        {

            List<Slip> slips = db.Slips.Where(s => s.DockID == dockId).Include(s => s.Dock).OrderBy(s => s.ID).ToList();
            return slips;
        }

        /// <summary>
        /// get slip with given ID
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the slip to find</param>
        /// <returns>slip or null if not found</returns>
        public static Slip GetSlipById(InlandMarinaContext db, int id)
        {
            Slip slip = db.Slips.Find(id);
            return slip;

        }

       

        /// <summary>
        /// get slips that have their ID attached to a lease
        /// </summary>
        /// <param name="leases">the list of leases</param>
        /// <returns>list of slips</returns>
        public static List<Slip> GetSlipsByLeases(List<Lease> leases)
        {
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                List<int> slipIds = leases.Select(l => l.SlipID).ToList();
                return db.Slips
                    .Where(s => slipIds.Contains(s.ID))
                    .ToList();
            }
        }

        

    }

}

