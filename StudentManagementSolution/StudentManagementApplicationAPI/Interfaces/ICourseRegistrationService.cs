using StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs;

namespace StudentManagementApplicationAPI.Interfaces
{
    public interface ICourseRegistrationService
    {
        public Task<CourseRegistrationReturnDTO> AddCourse(CourseRegistrationAddDTO courseRegistrationAddDTO);
        public Task<IEnumerable<CourseRegistrationReturnDTO>> GetAllCourseRegistrations();
        public Task<CourseRegistrationReturnDTO> GetCourseRegistrationById(int courseRegistrationId);
        public Task<CourseRegistrationReturnDTO> UpdateCourseRegistraion(int courseRegistrationId, int courseId);
        public Task<CourseRegistrationReturnDTO> DeleteCourseRegistration(int courseRegistrationId);
        public Task<IEnumerable<CourseRegistrationReturnDTO>> GetCoursesRegisteredByStudent(int studentId);
        public Task<IEnumerable<CourseRegistrationReturnDTO>> GetRegisteredStudentsForCourse(int courseId);
        public Task<CourseRegistrationReturnDTO> ApproveCourseRegistrations(int courseRegistrationId);
        public Task<IEnumerable<CourseRegistrationReturnDTO>> ApproveCourseRegistrationsForStudent(int studentId);
    }
}
