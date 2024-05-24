namespace StudentManagementApplicationAPI.Models.DTOs.CourseDTOs
{
    public class CourseReturnDTO
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int FacultyId { get; set; }
    }
}
