using StudentManagementApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.ExamDTOs
{
    public class ExamReturnDTO
    {

        public int ExamId { get; set; }
        [Required]
        public int CourseId { get; set; }

        [Range(0, 100)]
        public int TotalMark { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public string ExamType { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
