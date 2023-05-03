using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMarinaData
{
    public class CustomerManager
    {
        /// <summary>
        /// Class is responsible for authenticating and managing customers.
        /// </summary>
        
            private readonly static List<Customer> _customers;




        /// <summary>
        /// add another movie to the table
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="movie">new movie to add</param>
        public static void AddCustomer(Customer customer)
        {
            InlandMarinaContext db = new InlandMarinaContext();
            db.Customers.Add(customer);
            db.SaveChanges();
        }


        /// <summary>
        /// User is authenticated based on credentials and a user returned if exists or null if not.
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <param name="password">Password as string</param>
        /// <returns>A user object or null.</returns>
        /// <remarks>
        /// Add additional for the docs for this application--for developers.
        /// </remarks>
        public static Customer Authenticate(string username, string password)
            {
                Customer customer = null;
                using (InlandMarinaContext db = new InlandMarinaContext())
                {
                    customer = db.Customers.SingleOrDefault(cust => cust.Username == username
                                                        && cust.Password == password);
                }

                return customer; //this will either be null or an object
            }

        
    }
}
