using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniqueClassesSite.Models;

namespace UniqueClassesSite.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // १. कोर्सनुसार विद्यार्थ्यांची यादी दाखवणे (GET)
        // 📅 हजेरीचे पेज उघडणे (GET)
        public IActionResult TakeAttendance()
        {
            var today = DateTime.Today;

            // 🔍 कोर्सचे बंधन न ठेवता डेटाबेसमधील सर्व विद्यार्थी थेट आणणे:
            var allStudents = _context.Students.ToList();

            var existingAttendance = _context.DailyAttendances
                                             .Where(a => a.AttendanceDate.Date == today)
                                             .ToList();

            var viewModel = new AttendanceVM
            {
                AttendanceDate = today,
                StudentList = new List<StudentAttendanceViewModel>()
            };

            foreach (var student in allStudents)
            {
                var clonedRecord = existingAttendance.FirstOrDefault(a => a.StudentId == student.StudentId);
                viewModel.StudentList.Add(new StudentAttendanceViewModel
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    IsPresent = clonedRecord != null ? clonedRecord.IsPresent : true
                });
            }

            return View(viewModel);
        }
        // २. सर्व विद्यार्थ्यांची हजेरी एकत्र सेव्ह करणे (POST)
        [HttpPost]
       
        public IActionResult SaveDailyAttendance(AttendanceVM model)
        {
            if (model.StudentList != null && model.StudentList.Any())
            {
                foreach (var student in model.StudentList)
                {
                    var existing = _context.DailyAttendances
                        .FirstOrDefault(a => a.AttendanceDate.Date == model.AttendanceDate.Date && a.StudentId == student.StudentId);

                    if (existing != null)
                    {
                        existing.IsPresent = student.IsPresent;
                    }
                    else
                    {
                        var newAttendance = new DailyAttendance
                        {
                            StudentId = student.StudentId,
                            AttendanceDate = model.AttendanceDate,
                            IsPresent = student.IsPresent
                        };
                        _context.DailyAttendances.Add(newAttendance);
                    }
                }
                _context.SaveChanges();
                TempData["Success"] = "हजेरी यशस्वीरित्या सेव्ह झाली!";
            }

            // 🔄 थेट हजेरी इतिहास (AttendanceHistory) पेजवर पाठवणे:
            return RedirectToAction("AttendanceHistory");
        }
        public IActionResult StudentList()
        {
            // डेटाबेसमधून सर्व विद्यार्थ्यांची यादी ओढणे
            var allStudents = _context.Students.ToList();
            return View(allStudents);
        }


        // 📅 सर्व हजेरीचा इतिहास (Attendance Report) दाखवणे
        public IActionResult AttendanceHistory()
        {
            // DailyAttendances टेबलमधून डेटा आणताना सोबत Students टेबलचा डेटा देखील जॉइन (Include) करणे
            var history = _context.DailyAttendances
                                  .Include(a => a.Student)
                                  .OrderByDescending(a => a.AttendanceDate) // नवीन तारीख आधी दाखवणे
                                  .ToList();

            return View(history);
        }
    

    public IActionResult EditAttendance(DateTime date, string course = "Advanced Software Development")
        {
            // त्या तारखेला आणि त्या कोर्ससाठी घेतलेली हजेरी डेटाबेसमधून शोधणे
            var attendanceRecords = _context.DailyAttendances
                                            .Include(a => a.Student)
                                            .Where(a => a.AttendanceDate.Date == date.Date && a.Student.CourseName == course)
                                            .ToList();

            if (!attendanceRecords.Any())
            {
                TempData["Error"] = "या तारखेचा कोणताही हजेरी रेकॉर्ड सापडला नाही!";
                return RedirectToAction("AttendanceHistory");
            }

            // व्ह्यू-मॉडेलमध्ये डेटा भरणे
            var viewModel = new AttendanceVM
            {
                AttendanceDate = date,
                SelectedCourse = course,
                StudentList = attendanceRecords.Select(r => new StudentAttendanceViewModel
                {
                    StudentId = r.StudentId,
                    StudentName = r.Student != null ? r.Student.StudentName : "Unknown",
                    IsPresent = r.IsPresent
                }).ToList()
            };

            return View(viewModel);
        }
        // 🗑️ विशिष्ट तारखेची हजेरी डिलीट करणे
        public IActionResult DeleteAttendance(DateTime date, string course)
        {
            // १. त्या तारखेचे आणि त्या कोर्सचे सर्व हजेरी रेकॉर्ड्स शोधणे
            var recordsToDelete = _context.DailyAttendances
                                          .Include(a => a.Student)
                                          .Where(a => a.AttendanceDate.Date == date.Date && a.Student.CourseName == course)
                                          .ToList();

            if (recordsToDelete.Any())
            {
                // २. डेटाबेसमधून सर्व रेकॉर्ड्स काढून टाकणे
                _context.DailyAttendances.RemoveRange(recordsToDelete);
                _context.SaveChanges();

                TempData["Success"] = "निवडलेल्या तारखेची हजेरी यशस्वीरित्या डिलीट केली आहे!";
            }
            else
            {
                TempData["Error"] = "डिलीट करण्यासाठी कोणताही रेकॉर्ड सापडला नाही!";
            }

            // पुन्हा हजेरी रिपोर्ट पेजवर जाणे
            return RedirectToAction("AttendanceHistory");
        }

        // २. बदललेली हजेरी डेटाबेसमध्ये अपडेट करणे (POST)
        [HttpPost]
        [HttpPost]
        public IActionResult UpdateDailyAttendance(AttendanceVM model)
        {
            if (model.StudentList != null)
            {
                foreach (var item in model.StudentList)
                {
                    var existingRecord = _context.DailyAttendances
                        .FirstOrDefault(a => a.AttendanceDate.Date == model.AttendanceDate.Date && a.StudentId == item.StudentId);

                    if (existingRecord != null)
                    {
                        existingRecord.IsPresent = item.IsPresent;
                    }
                }
                _context.SaveChanges();
                TempData["Success"] = "हजेरी यशस्वीरित्या अपडेट झाली आहे!";
            }

            // 🔄 रिडायरेक्ट टू इतिहास पेज
            return RedirectToAction("AttendanceHistory");
        }
        // १. विद्यार्थी जोडण्याचे पेज उघडणे (GET)
        public IActionResult AddStudent()
        {
            return View();
        }

        // २. फॉर्म सबमिट झाल्यावर डेटाबेसमध्ये सेव्ह करणे (POST)
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                TempData["Success"] = "नवीन विद्यार्थी यशस्वीरित्या जोडला गेला आहे!";
                return RedirectToAction("StudentList");
            }
            return View(student);
        }
        [HttpPost]
        public IActionResult SaveSingleStudentAttendance(int studentId, string date, bool isPresent)
        {
            try
            {
                // १. स्ट्रिंग तारखेला DateTime मध्ये रूपांतरित करणे
                DateTime attendanceDate = DateTime.Parse(date);

                // २. आजच्या तारखेला या विद्यार्थ्याचा रेकॉर्ड आधीच आहे का तपासणे
                var existing = _context.DailyAttendances
                    .FirstOrDefault(a => a.AttendanceDate.Date == attendanceDate.Date && a.StudentId == studentId);

                if (existing != null)
                {
                    // रेकॉर्ड असेल तर फक्त स्टेटस अपडेट करा
                    existing.IsPresent = isPresent;
                }
                else
                {
                    // रेकॉर्ड नसेल तर नवीन एन्ट्री टाका
                    var newAttendance = new DailyAttendance
                    {
                        StudentId = studentId,
                        AttendanceDate = attendanceDate,
                        IsPresent = isPresent
                    };
                    _context.DailyAttendances.Add(newAttendance);
                }

                _context.SaveChanges();
                return Json(new { success = true, message = "Status Updated!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // 📊 एका विशिष्ट दिवसाचा हजेरी अहवाल (Daily Report)
        public IActionResult DailyReport(DateTime? date)
        {
            // जर तारीख निवडलेली नसेल, तर बाय डिफॉल्ट आजची तारीख घेणे
            DateTime filterDate = date ?? DateTime.Today;

            // निवडलेल्या तारखेचा सर्व हजेरी डेटा डेटाबेसमधून आणणे
            var dailyRecords = _context.DailyAttendances
                                       .Include(a => a.Student)
                                       .Where(a => a.AttendanceDate.Date == filterDate.Date)
                                       .ToList();

            // आकडेवारी मोजणे (Total Present & Absent Count)
            ViewBag.TotalPresent = dailyRecords.Count(r => r.IsPresent);
            ViewBag.TotalAbsent = dailyRecords.Count(r => !r.IsPresent);
            ViewBag.SelectedDate = filterDate.ToString("yyyy-MM-dd");

            return View(dailyRecords);
        }
    } }