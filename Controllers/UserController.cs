using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using System.Linq;

namespace UFISApp.Controllers
{
    public class UserController : Controller 
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        // create method to validate user credentials 
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                using (LoginContext db = new LoginContext())
                {
                    // make sure username and password both match
                    var obj = db.Users.Where(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).FirstOrDefault();

                        // redirect to successful login page
                        if (obj != null)
                        {
                            Session["UserID"] = obj.UserId.ToString();  
                            Session["UserName"] = obj.Username.ToString();  
                            return RedirectToAction("SuccessfulLogin");
                        }
                        else 
                        {
                            user.LoginErrorMessage = "Wrong username or password."; 
                        }
                }
            } 
            return View(user);
        }

        // create method to redirect user after successful login
        public ActionResult SuccessfulLogin()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else 
            {
                return RedirectToAction("Login");
            }
        }
    }
}