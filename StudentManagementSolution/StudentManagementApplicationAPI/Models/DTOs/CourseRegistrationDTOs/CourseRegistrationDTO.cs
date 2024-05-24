namespace StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs
{
    public class CourseRegistrationDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
