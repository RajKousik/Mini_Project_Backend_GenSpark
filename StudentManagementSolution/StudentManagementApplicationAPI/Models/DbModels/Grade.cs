using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        
        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        
        public int EvaluatedById { get; set; }

        [ForeignKey("EvaluatedById")]
        public Faculty EvaluatedBy { get; set; }

        [Range(0, 100)]
        public int MarksScored { get; set; }

        [Range(0, 100)]
        public double Percentage { get; set; }

        [Required]
        [StringLength(1)]
        public GradeType StudentGrade { get; set; }

        public string Comments { get; set; }
    }
}
