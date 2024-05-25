using StudentManagementApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.ExamDTOs
{
    public class ExamDTO
    {
        [Required]
        public int CourseId { get; set; }

        [Range(0, 100)]
        public int TotalMark { get; set; }

        [Required]
        public DateOnly ExamDate { get; set; }

        [Required]
        public string ExamType { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
