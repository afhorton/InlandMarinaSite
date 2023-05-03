using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMarinaData
{
    public class LeaseManager
    {
        /// <summary>
        /// adds lease to database
        /// </summary>
        /// <param name="lease">lease to add</param>
        public static void Add(InlandMarinaContext db, Lease lease)
        {
            db.Leases.Add(lease);
            db.SaveChanges();
        }

        
        /// <summary>
        /// this gets the leases for each customer who is logged in
        /// </summary>
        /// <param name="customerId">the id of the logged in customer</param>
        /// <returns>a list of leases</returns>
        public static List<Lease> GetLeasesByCustomer(int customerId)
        {
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                return db.Leases
                    .Where(l => l.CustomerID == customerId)
                    .Include(l => l.Customer)
                    .Include(l => l.Slip)
                    .ToList();
            }
        }

     
    }
}
