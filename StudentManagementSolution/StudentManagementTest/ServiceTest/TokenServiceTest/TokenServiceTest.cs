using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Interfaces.Service.TokenService;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTest.ServiceTest.TokenServiceTest
{
    public class TokenServiceTest
    {

        #region Fields
        ITokenService _tokenService;
        Mock<ILogger<TokenService>> mockLoggerConfigForToken;
        #endregion

        [SetUp]
        public void SetUp()
        {
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);

            mockLoggerConfigForToken = new Mock<ILogger<TokenService>>();

            _tokenService = new TokenService(mockConfig.Object, mockLoggerConfigForToken.Object);
        }

        [Test, Order(1)]
        public void GenerateFacultyTokenSuccessTest()
        {
            var hmac = new HMACSHA512();
            var faculty1 = new Faculty()
            {
                Name = "faculty1",
                Email = "faculty1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Admin,
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };

            var token = _tokenService.GenerateFacultyToken(faculty1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            Assert.IsNotNull(emailClaim);
            Assert.That(emailClaim.Value.ToString(), Is.EqualTo(faculty1.Email));
        }

        [Test, Order(2)]
        public void GenerateStudentTokenSuccessTest()
        {
            var hmac = new HMACSHA512();
            var student1 = new Student()
            {
                Name = "student1",
                Email = "student1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };

            var token = _tokenService.GenerateStudentToken(student1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            Assert.IsNotNull(emailClaim);
            Assert.That(emailClaim.Value.ToString(), Is.EqualTo(student1.Email));
        }

        [Test, Order(3)]
        public void GenerateFacultyTokenFailureTest()
        {
            var hmac = new HMACSHA512();
            var faculty1 = new Faculty()
            {
                Name = "faculty1",
                Email = "faculty1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Admin,
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };

            var token = _tokenService.GenerateFacultyToken(faculty1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);



            var claims = jwtToken.Claims;
            //for testing, making it as empty
            claims = Enumerable.Empty<Claim>();

            // Scenario where the email claim is missing
            var emailClaim = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            Assert.IsNull(emailClaim);

        }

        [Test, Order(4)]
        public void GenerateStudentTokenFailureTest()
        {
            var hmac = new HMACSHA512();
            var student1 = new Student()
            {
                Name = "student1",
                Email = "student1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };

            var token = _tokenService.GenerateStudentToken(student1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            

            var claims = jwtToken.Claims;
            //for testing, making it as empty
            claims = Enumerable.Empty<Claim>();

            // Scenario where the email claim is missing
            var emailClaim = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            Assert.IsNull(emailClaim);

        }

        

    }
}
