using AutoMapper;
using Easy_Password_Validator;
using Easy_Password_Validator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service.AuthService;
using StudentManagementApplicationAPI.Interfaces.Service.TokenService;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using StudentManagementApplicationAPI.Services.Faculty_Service;
using StudentManagementApplicationAPI.Services.Token;
using System.Security.Cryptography;
using System.Text;


namespace StudentManagementTest.ServiceTest.FacultyAuthServiceTest
{
    public class FacultyAuthServiceTest
    {
        #region Fields

        StudentManagementContext context;
        ITokenService _tokenService;
        IRepository<int, Faculty> _facultyRepo;
        IRepository<int, Department> _departmentRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        PasswordValidatorService passwordValidatorService;
        Mock<IConfiguration> mockPasswordConfig;
        Mock<ILogger<FacultyAuthService>> mockLoggerConfig; 
        Mock<ILogger<TokenService>> mockLoggerConfigForToken;

        #endregion

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyFacultyAuthDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _facultyRepo = new FacultyRepository(context);
            _departmentRepo = new DepartmentRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "StudentManagementApplicationAPI"
            }));
            _mapper = _config.CreateMapper();

            mockLoggerConfigForToken = new Mock<ILogger<TokenService>>();

            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("Lk3xG9pVqRmZtRwYk7oPnTjWrAsDfGhUi8yBnJkLm9zXx2cVnMl0pOu1tZr4eDcFvGbHnJm5sR3Zn9JyQaPx7oWtUgXhIvDcFeGbVkLmOpNjRbEaUcPy8x6y0Zq4w1u3t5r7i9w2");
            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);
            _tokenService = new TokenService(mockConfig.Object, mockLoggerConfigForToken.Object);

            Mock<IConfigurationSection> passwordValueConfig = new Mock<IConfigurationSection>();
            passwordValueConfig.Setup(x => x.Value).Returns("true");
            mockPasswordConfig = new Mock<IConfiguration>();
            mockPasswordConfig.Setup(x => x.GetSection("AllowPasswordValidation")).Returns(passwordValueConfig.Object);

            passwordValidatorService = new PasswordValidatorService(new PasswordRequirements());

            mockLoggerConfig = new Mock<ILogger<FacultyAuthService>>();
        }

        private async Task SeedDatabaseAsync()
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
            var faculty4 = new Faculty()
            {
                Name = "faculty4",
                Email = "faculty4@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Admin,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty4")),
                DepartmentId = 1,
            };
            await context.Faculties.AddRangeAsync(faculty1, faculty4);

            var department1 = new Department { Name = "Computer Science", HeadId = 1 };

            await context.Departments.AddRangeAsync(department1);


            var student1 = new Student
            {
                Name = "student1",
                Email = "student1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("student1")),
                DepartmentId = 1
            };

            await context.Students.AddRangeAsync(student1);

            await context.SaveChangesAsync();
        }

        [Test, Order(1)]
        public async Task LoginSuccessTest()
        {

            await SeedDatabaseAsync();

            IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO> facultyLoginService = new FacultyAuthService(_tokenService, _facultyRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object);

            FacultyLoginDTO facultyLoginDTO = new FacultyLoginDTO
            {
                Email = "faculty1@gmail.com",
                Password = "faculty1"
            };

            FacultyLoginReturnDTO result = await facultyLoginService.Login(facultyLoginDTO);

            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(facultyLoginDTO.Email));

        }

        [Test, Order(2)]
        public void LoginFailureTest()
        {

            IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO> facultyLoginService = new FacultyAuthService(_tokenService, _facultyRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object);

            FacultyLoginDTO facultyLoginDTO = new FacultyLoginDTO
            {
                Email = "faculty1@gmail.com",
                Password = "wrong password"
            };

            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await facultyLoginService.Login(facultyLoginDTO));

            Assert.ThrowsAsync<UserNotActivatedException>(async () => await facultyLoginService.Login(new FacultyLoginDTO
            {
                Email = "faculty4@gmail.com",
                Password = "faculty4"
            }));

        }

        [Test, Order(3)]
        public async Task RegisterSuccessTest()
        {

            IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO> facultyResgiterService = new FacultyAuthService(_tokenService, _facultyRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object);

            FacultyRegisterDTO facultyRegisterDTO = new FacultyRegisterDTO
            {
                Name = "faculty2",
                Email = "faculty2@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                DepartmentId = 1,
                Password = "faculty2"
            };

            var result = await facultyResgiterService.Register(facultyRegisterDTO, RoleType.Associate_Proffesors);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(facultyRegisterDTO.Name));

        }

        [Test, Order(4)]
        public void RegisterFailureTest()
        {

            IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO> facultyResgiterService = new FacultyAuthService(_tokenService, _facultyRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object);

            FacultyRegisterDTO facultyRegisterDTO = new FacultyRegisterDTO
            {
                Name = "faculty1",
                Email = "faculty1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                DepartmentId = 1,
                Password = "faculty1"
            };

            Assert.ThrowsAsync<DuplicateEmailException>(async () => await facultyResgiterService.Register(facultyRegisterDTO, RoleType.Associate_Proffesors));

        }
    }
}
