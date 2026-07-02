using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniqueClassesSite.Models;

namespace UniqueClassesSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Contactus()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // डेटाबेसमधून ठराविक आयडी (ID) असलेला कोर्स शोधणे
            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        public IActionResult Index()
        {
            // SQL Server मधील कोर्सेसची लिस्ट आणणे
            var courseList = _context.Courses.ToList();
            return View(courseList);
        }
        [HttpPost]
        public async Task<IActionResult> SendEnquiry(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                // डेटाबेसमध्ये सेव्ह करण्यासाठी लॉजिक
                _context.Enquiries.Add(enquiry);
                await _context.SaveChangesAsync();

                // यशस्वी मेसेज दाखवण्यासाठी (Optional)
                TempData["Success"] = "तुमचा मेसेज मिळाला आहे. आम्ही लवकरच संपर्क करू!";

                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}