using Microsoft.IdentityModel.Tokens;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementApplicationAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        private string CreateToken(IEnumerable<Claim> claims)
        {
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateFacultyToken(Faculty faculty)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, faculty.FacultyId.ToString()),
                new Claim(ClaimTypes.Role, faculty.Role.ToString()),
                new Claim(ClaimTypes.Email, faculty.Email),
                new Claim("FullName", faculty.Name)
            };

            return CreateToken(claims);
        }

        public string GenerateStudentToken(Student student)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, student.StudentRollNo.ToString()),
                new Claim(ClaimTypes.Role, RoleType.Student.ToString()),            
                new Claim(ClaimTypes.Email, student.Email),
                new Claim("FullName", student.Name)
            };

            return CreateToken(claims);
        }
    }

}
