namespace StudentManagementApplicationAPI.Models.DTOs.CourseDTOs
{
    public class CourseUpdateDTO
    {
        public string? Description { get; set; }
        public int FacultyId { get; set; }

        public double CourseFees { get; set; }
        public int CourseVacancy { get; set; }
    }
}
