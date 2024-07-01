using StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;

namespace StudentManagementApplicationAPI.Interfaces.Service
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

        public Task<IEnumerable<StudentReturnDTO>> GetApprovedStudentsByCourse(int courseId);

        public Task<CourseRegistrationReturnDTO> ApproveCourseRegistrations(int courseRegistrationId);

        public Task<CourseRegistrationReturnDTO> RejectCourseRegistrations(int courseRegistrationId);
        public Task<IEnumerable<CourseRegistrationReturnDTO>> ApproveCourseRegistrationsForStudent(int studentId);

        public Task<IEnumerable<CourseRegistrationReturnDTO>> GetCoursesRegisteredByStudentAndStatus(int studentId, int status);
    }
}
