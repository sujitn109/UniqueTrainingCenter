using Microsoft.AspNetCore.Mvc;
using UniqueClassesSite.Models;

namespace UniqueClassesSite.Controllers
{
    public class Account : Controller
    {
        // लॉगिन पेज दाखवण्यासाठी
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // लॉगिन प्रक्रिया हाताळण्यासाठी
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // सुजित ननावरे किंवा सुमिता ननावरे यांच्यासाठी सुरक्षित तपासणी
                if (model.Username == "admin_unique" && model.Password =="12345")
                {
                    // लॉगिन यशस्वी झाल्यावर डॅशबोर्डवर रिडायरेक्ट करा
                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError("", "अवैध युझरनेम किंवा पासवर्ड.");
            }
            return View(model);
        }

        // लॉगआउट प्रक्रिया
        public IActionResult Logout()
        {
            // सेशन क्लिअर करा
            return RedirectToAction("Login");
        }
    }
}

