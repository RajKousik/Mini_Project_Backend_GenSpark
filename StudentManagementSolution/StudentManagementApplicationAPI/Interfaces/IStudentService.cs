using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;

namespace StudentManagementApplicationAPI.Interfaces
{
    public interface IStudentService
    {
        public Task<StudentDTO> UpdateStudent(StudentDTO dto, string email);
        public Task<StudentDTO> DeleteStudent(string email);
        public Task<StudentDTO> GetStudentById(int studentRollNo);
        public Task<StudentDTO> GetStudentByEmail(string email);

        public Task<IEnumerable<StudentDTO>> GetStudentByName(string name);
        public Task<IEnumerable<StudentDTO>> GetAllStudents();
        public Task<IEnumerable<StudentDTO>> GetStudentsByDepartment(int departmentId);
    }
}
