using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Services;
using System.Security.Claims;

namespace StudentManagementApplicationAPI.Interfaces.Service.TokenService
{
    public interface ITokenService
    {
        public string GenerateFacultyToken(Faculty faculty);
        public string GenerateStudentToken(Student student);

        //public void AddLoggedOutClaim(Claim claim, TokenManagerMiddleware middleware);
    }
}
