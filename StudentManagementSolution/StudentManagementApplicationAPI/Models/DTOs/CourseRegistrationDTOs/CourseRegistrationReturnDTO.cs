using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs
{
    public class CourseRegistrationReturnDTO
    {
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Comments { get; set; }
    }
}
