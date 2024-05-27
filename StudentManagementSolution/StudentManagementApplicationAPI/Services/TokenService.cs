using Microsoft.IdentityModel.Tokens;
using StudentManagementApplicationAPI.Interfaces.Service.TokenService;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementApplicationAPI.Services
{
    public class TokenService : ITokenService
    {
        #region Private Fields

        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;
        private readonly ILogger<TokenService> _logger;

        #endregion

        #region Constructor
        #region Summary
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the JWT secret key.</param>
        #endregion
        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            _logger = logger;
        }
        #endregion

        #region Private Methods
        #region Summary
        /// <summary>
        /// Creates a JWT token based on the provided claims.
        /// </summary>
        /// <param name="claims">The claims to include in the token.</param>
        /// <returns>A JWT token as a string.</returns>
        #endregion
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
        #endregion

        #region Public Methods
        #region Summary
        /// <summary>
        /// Generates a JWT token for a faculty member.
        /// </summary>
        /// <param name="faculty">The faculty member for whom to generate the token.</param>
        /// <returns>A JWT token as a string.</returns>
        #endregion
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

        #region Summary
        /// <summary>
        /// Generates a JWT token for a student.
        /// </summary>
        /// <param name="student">The student for whom to generate the token.</param>
        /// <returns>A JWT token as a string.</returns>
        #endregion
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
        #endregion
    }

}
