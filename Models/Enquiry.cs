using System.ComponentModel.DataAnnotations;

namespace UniqueClassesSite.Models
{
    public class Enquiry
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "कृपया तुमचे पूर्ण नाव टाका.")]
        [StringLength(50, ErrorMessage = "नाव ५० अक्षरांपेक्षा जास्त नसावे.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "संपर्क माहिती (मोबाईल किंवा ईमेल) आवश्यक आहे.")]
        [Display(Name = "मोबाईल नंबर / ईमेल")]
        // मोबाईल नंबर किंवा ईमेल दोन्ही स्वीकारण्यासाठी हे व्हॅलिडेशन:
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "कृपया तुमचा प्रश्न किंवा संदेश लिहा.")]
        [MinLength(5, ErrorMessage = "संदेश किमान ५ अक्षरांचा असावा.")]
        public string Message { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;
    }
}