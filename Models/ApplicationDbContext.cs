using Microsoft.EntityFrameworkCore;

namespace UniqueClassesSite.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // कोर्सेससाठी टेबल तयार होईल
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Student> Students { get; set; } // 👈 हे जोडा
        public DbSet<DailyAttendance> DailyAttendances { get; set; } // 👈 हे जोडा
        public class StudentAttendanceViewModel
        {
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public bool IsPresent { get; set; }
        }

    }
}

