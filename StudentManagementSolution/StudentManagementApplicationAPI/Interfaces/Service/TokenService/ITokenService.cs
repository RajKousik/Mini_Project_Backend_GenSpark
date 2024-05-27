using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Interfaces.Service.TokenService
{
    public interface ITokenService
    {
        public string GenerateFacultyToken(Faculty faculty);
        public string GenerateStudentToken(Student student);
    }
}
