namespace StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs
{
    public class CourseRegistrationReturnDTO
    {
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
