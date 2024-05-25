using StudentManagementApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.GradeDTOs
{
    public class GradeReturnDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int EvaluatedById { get; set; }
        public int MarksScored { get; set; }
        public double Percentage { get; set; }
        public String StudentGrade { get; set; }
        public string Comments { get; set; }
    }
}
