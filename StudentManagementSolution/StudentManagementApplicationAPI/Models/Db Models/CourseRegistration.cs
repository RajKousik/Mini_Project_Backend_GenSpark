using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    public class CourseRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public bool IsApproved { get; set; }

        public string Comments { get; set; }
    }

}
