using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqueClassesSite.Models;

namespace UniqueClassesSite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // सर्व कोर्सेसची लिस्ट पाहण्यासाठी (Dashboard)
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        public IActionResult ViewStudentDashboard()
        {
            // विद्यार्थ्यांच्या मुख्य पेजवर रिडायरेक्ट करा
            return RedirectToAction("Index", "Home");
        }
        // नवीन माहिती भरण्यासाठी पेज उघडणेx
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // माहिती सेव्ह करणे
        [HttpPost]
        public async Task<IActionResult> Create(Course course, IFormFile posterFile, IFormFile videoFile)
        {
            if (posterFile != null)
            {
                var posterName = Guid.NewGuid().ToString() + "_" + posterFile.FileName;

                // फोल्डरचा पाथ तयार करा
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "posters");

                // जर फोल्डर नसेल, तर ते तयार करा
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var savePath = Path.Combine(folderPath, posterName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await posterFile.CopyToAsync(stream);
                }
                course.PosterPath = "/uploads/posters/" + posterName;
            }

            if (videoFile != null)
            {
                var videoName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;

                // व्हिडिओ फोल्डरचा पाथ आणि चेक
                var videoFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");

                if (!Directory.Exists(videoFolderPath))
                {
                    Directory.CreateDirectory(videoFolderPath);
                }

                var savePath = Path.Combine(videoFolderPath, videoName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }
                course.VideoPath = "/uploads/videos/" + videoName;
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // १. डिलीट करण्यापूर्वी खात्री करण्यासाठी (Confirm Page)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) return NotFound();

            return View(course);
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

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course, IFormFile? posterFile, IFormFile? videoFile)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // जर नवीन पोस्टर अपलोड केले असेल तर
                    if (posterFile != null)
                    {
                        var posterName = Guid.NewGuid().ToString() + "_" + posterFile.FileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "posters", posterName);
                        using (var stream = new FileStream(path, FileMode.Create)) { await posterFile.CopyToAsync(stream); }
                        course.PosterPath = "/uploads/posters/" + posterName;
                    }

                    // जर नवीन व्हिडिओ अपलोड केला असेल तर
                    if (videoFile != null)
                    {
                        var videoName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos", videoName);
                        using (var stream = new FileStream(path, FileMode.Create)) { await videoFile.CopyToAsync(stream); }
                        course.VideoPath = "/uploads/videos/" + videoName;
                    }

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        private bool CourseExists(int id) => _context.Courses.Any(e => e.Id == id);
        // २. प्रत्यक्ष डिलीट करण्यासाठी
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Inquiries()
        {
            // नवीन चौकशी सर्वात वर दिसण्यासाठी OrderByDescending वापरले आहे
            var inquiries = await _context.Enquiries.OrderByDescending(e => e.SubmittedDate).ToListAsync();
            return View(inquiries);
        }
        public IActionResult Messages()
        {
            // नवीन मेसेज वर दिसण्यासाठी 'OrderByDescending' वापरले आहे
            var messages = _context.Enquiries.OrderByDescending(m => m.SubmittedDate).ToList();
            return View(messages);
        }
    }
}