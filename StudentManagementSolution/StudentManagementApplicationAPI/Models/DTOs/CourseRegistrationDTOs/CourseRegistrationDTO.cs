using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs
{
    public class CourseRegistrationDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Comments { get; set; }
    }
}
