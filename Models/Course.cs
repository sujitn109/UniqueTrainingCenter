namespace UniqueClassesSite.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "कोर्सचे नाव आवश्यक आहे")]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "माहिती भरणे गरजेचे आहे")]
        public string Description { get; set; }

        [Range(1, 100000, ErrorMessage = "फीस योग्य आकड्यात असावी")]
        public decimal Fees { get; set; }

        public string? Duration { get; set; }
        public string? PosterPath { get; set; }
        public string? VideoPath { get; set; }
    }
}
