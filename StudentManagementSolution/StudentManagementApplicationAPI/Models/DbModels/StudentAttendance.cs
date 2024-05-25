using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    public class StudentAttendance
    {
        [Key]
        public int ID { get; set; }
        public int StudentRollNo { get; set; }

        [ForeignKey("StudentRollNo")]
        public Student Student { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public AttendanceStatus AttendanceStatus { get; set; }
    }
}
