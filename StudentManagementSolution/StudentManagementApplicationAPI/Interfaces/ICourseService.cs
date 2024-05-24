using StudentManagementApplicationAPI.Models.DTOs.CourseDTOs;

namespace StudentManagementApplicationAPI.Interfaces
{
    public interface ICourseService
    {
        public Task<CourseReturnDTO> AddCourse(CourseDTO courseDTO);
        public Task<IEnumerable<CourseReturnDTO>> GetAllCourses();
        public Task<CourseReturnDTO> GetCourseById(int courseId);
        public Task<IEnumerable<CourseReturnDTO>> GetCourseByName(string name);
        public Task<IEnumerable<CourseReturnDTO>> GetCourseByFaculty(int facultyId);
        public Task<CourseReturnDTO> UpdateCourse(int courseId, CourseDTO courseUpdateDTO);
    }
}
