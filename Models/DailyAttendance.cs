using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueClassesSite.Models
{
    public class DailyAttendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public DateTime AttendanceDate { get; set; } = DateTime.Now;
        public bool IsPresent { get; set; }

        // Navigation property
        public Student Student { get; set; }
    }
}
