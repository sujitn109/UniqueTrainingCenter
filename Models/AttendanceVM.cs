namespace UniqueClassesSite.Models
{
    public class AttendanceVM
    {
        public DateTime AttendanceDate { get; set; } = DateTime.Today;
        public string SelectedCourse { get; set; }
       // public List<StudentAttendanceSelection> StudentList { get; set; } = new List<StudentAttendanceSelection>();
        public List<StudentAttendanceViewModel> StudentList { get; set; } = new List<StudentAttendanceViewModel>();

    }
    public class StudentAttendanceViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool IsPresent { get; set; }
    }
}
