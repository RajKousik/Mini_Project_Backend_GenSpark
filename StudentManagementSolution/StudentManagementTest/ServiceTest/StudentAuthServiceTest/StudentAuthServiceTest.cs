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
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementTest.ServiceTest.StudentAuthServiceTest
{
    public class StudentAuthServiceTest
    {
        #region Fields
        StudentManagementContext context;
        ITokenService _tokenService;
        IRepository<int, Student> _studentRepo;
        IRepository<int, Department> _departmentRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        PasswordValidatorService passwordValidatorService;
        Mock<IConfiguration> mockPasswordConfig;
        Mock<ILogger<StudentAuthService>> mockLoggerConfig;
        Mock<ILogger<TokenService>> mockLoggerConfigForToken;
        #endregion

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyStudentAuthDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _studentRepo = new StudentRepository(context);
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

            Mock<IConfigurationSection> passwordValueConfig = new Mock<IConfigurationSection>();
            passwordValueConfig.Setup(x => x.Value).Returns("true");
            mockPasswordConfig = new Mock<IConfiguration>();
            mockPasswordConfig.Setup(x => x.GetSection("AllowPasswordValidation")).Returns(passwordValueConfig.Object);

            mockLoggerConfig = new Mock<ILogger<StudentAuthService>>();

            _tokenService = new TokenService(mockConfig.Object, mockLoggerConfigForToken.Object);

            passwordValidatorService = new PasswordValidatorService(new PasswordRequirements());
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
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };
            await context.Faculties.AddRangeAsync(faculty1);

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

            IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> studentLoginService = new StudentAuthService(_tokenService, _studentRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object);

            StudentLoginDTO studentLoginDTO = new StudentLoginDTO
            {
                Email = "student1@gmail.com",
                Password = "student1"
            };

            StudentLoginReturnDTO result = await studentLoginService.Login(studentLoginDTO);

            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(studentLoginDTO.Email));

        }

        [Test, Order(2)]
        public void LoginFailureTest()
        {

            //IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> studentLoginService = new StudentAuthService(_tokenService, _studentRepo, _mapper, _departmentRepo); ;
            IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> studentLoginService = new StudentAuthService(_tokenService, _studentRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object);

            StudentLoginDTO studentLoginDTO = new StudentLoginDTO
            {
                Email = "student1@gmail.com",
                Password = "student123"
            };

            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await studentLoginService.Login(studentLoginDTO));

        }

        [Test, Order(3)]
        public async Task RegisterSuccessTest()
        {

            IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> studentResgiterService = new StudentAuthService(_tokenService, _studentRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object); ;

            StudentRegisterDTO studentRegisterDTO = new StudentRegisterDTO
            {
                Name = "student2",
                Email = "student2@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                DepartmentId = 1,
                Password = "student2"
            };

            var result = await studentResgiterService.Register(studentRegisterDTO, RoleType.Student);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(studentRegisterDTO.Name));

        }

        [Test, Order(4)]
        public void RegisterFailureTest()
        {

            //IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> studentResgiterService = new StudentAuthService(_tokenService, _studentRepo, _mapper, _departmentRepo); ;
            IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> studentResgiterService = new StudentAuthService(_tokenService, _studentRepo, _mapper, _departmentRepo, passwordValidatorService, mockPasswordConfig.Object, mockLoggerConfig.Object); 

            StudentRegisterDTO studentRegisterDTO = new StudentRegisterDTO
            {
                Name = "student",
                Email = "student1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                DepartmentId = 1
            };

            Assert.ThrowsAsync<DuplicateEmailException>(async () => await studentResgiterService.Register(studentRegisterDTO, RoleType.Student));

        }
    }
}