using IMarinaData;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InlandMarina_MVC.Controllers
{
    public class AccountController : Controller
    {

        // Route: /Account/Login
        public IActionResult Login(string returnUrl = "")
        {
            if (returnUrl != null)
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer customer) // data collected on the form
        {
            Customer cust = CustomerManager.Authenticate(customer.Username, customer.Password);
            if (cust == null) // failed authentication
            {
                return View(); // stay on the login page
            }
            // usr != null   - authentication passed

            if (cust != null)
            {
                HttpContext.Session.SetInt32("CurrentCustomer", (int)cust.ID);

            }
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cust.FirstName),
                new Claim("FirstName", cust.FirstName),
               
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme); // use cookies authentication
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal); // generates authentication cookie
            // if no return URL, go to the home page
            if (string.IsNullOrEmpty(TempData["ReturnUrl"].ToString()))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(TempData["ReturnUrl"].ToString());
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            // release authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //remove current owner from the session
            HttpContext.Session.Remove("CurrentCustomer");


            return RedirectToAction("Index", "Home"); // go to the home page
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
