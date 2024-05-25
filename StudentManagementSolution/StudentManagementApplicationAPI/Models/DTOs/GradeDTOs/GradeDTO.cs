using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.GradeDTOs
{
    public class GradeDTO
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int ExamId { get; set; }
        [Required]
        public int EvaluatedById { get; set; }
        [Required]
        public int MarksScored { get; set; }
        [Required]
        public string Comments { get; set; }
    }
}
