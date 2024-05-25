using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    [Index(nameof(CourseId), IsUnique = true)]
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }
        
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Range(1, 100)]
        public int TotalMark { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public ExamType ExamType { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public IList<Grade> Grades { get; set; }
    }
}
