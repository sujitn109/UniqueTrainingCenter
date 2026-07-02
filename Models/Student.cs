using System.ComponentModel.DataAnnotations;

namespace UniqueClassesSite.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
    }
}
