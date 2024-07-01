using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;

namespace StudentManagementApplicationAPI.Interfaces.Service
{
    public interface IStudentService
    {
        public Task<StudentDTO> UpdateStudent(StudentDTO dto, string email);
        public Task<StudentDTO> DeleteStudent(string email);
        public Task<StudentReturnDTO> GetStudentById(int studentRollNo);
        public Task<StudentDTO> GetStudentByEmail(string email);

        public Task<IEnumerable<StudentDTO>> GetStudentByName(string name);
        public Task<IEnumerable<StudentReturnDTO>> GetAllStudents();
        public Task<IEnumerable<StudentReturnDTO>> GetStudentsByDepartment(int departmentId);
        public Task<StudentWalletReturnDTO> RechargeWallet(StudentWalletDTO studentWalletDTO);

        public Task<double> GetEWalletAmount(int studentRollNo);
    }
}
