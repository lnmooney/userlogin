using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace UFISApp.Controllers
{
    public class UserController : Controller 
    {
        private LoginContext db = new LoginContext();
        public ActionResult Index() 
        {
            return View();
        }

        // create register method
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {   
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        private static Logger logger = LogManager.GetLogger("myAppLoggerRules");

        // encrypt method
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        // decrypt method 
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
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