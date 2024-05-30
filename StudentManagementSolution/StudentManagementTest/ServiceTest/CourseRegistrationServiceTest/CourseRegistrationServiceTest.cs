using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Org.BouncyCastle.Crypto.Macs;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services.Course_Service;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementTest.ServiceTest.CourseRegistrationServiceTest
{
    public class CourseRegistrationServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Course> _courseRepo;
        IRepository<int, Student> _studentRepo;
        IRepository<int, CourseRegistration> _courseRegistrationRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        Mock<ILogger<CourseRegistrationService>> mockLoggerConfig;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyCourseRegistrationServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _courseRepo = new CourseRepository(context);
            _studentRepo = new StudentRepository(context);
            _courseRegistrationRepo = new CourseRegistrationRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "StudentManagementApplicationAPI"
            }));
            _mapper = _config.CreateMapper();

            mockLoggerConfig = new Mock<ILogger<CourseRegistrationService>>();
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
                Role = RoleType.Associate_Proffesors,
                Status = ActivationStatus.Active,
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
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("student1")),
                DepartmentId = 1,
                EWallet = 10000
            };

            await context.Students.AddRangeAsync(student1);

            var course1 = new Course
            {
                Name = "C#",
                Description = "This is a C# Course",
                FacultyId = 1,
                CourseVacancy = 10,
            };
            var course2 = new Course
            {
                Name = "MERN",
                Description = "This is a MERN Course",
                FacultyId = 1,
                CourseVacancy = 10,
            };
            var course3 = new Course
            {
                Name = "JAVA",
                Description = "This is a JAVA Course",
                FacultyId = 1,
                CourseVacancy = 10,
            };
            var course4 = new Course
            {
                Name = "CPP",
                Description = "This is a CPP Course",
                FacultyId = 1,
                CourseVacancy = 10,
            };
            await context.Courses.AddRangeAsync(course1, course2, course3, course4);
            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Students.RemoveRange(context.Students);
            //context.Faculties.RemoveRange(context.Faculties);
            context.Courses.RemoveRange(context.Courses);
            context.CourseRegistrations.RemoveRange(context.CourseRegistrations);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Success Tests

        [Test, Order(1)]
        public async Task AddCourseRegistrationSuccess()
        {
            await SeedDatabaseAsync();
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var courseRegistrationAddDTO = new CourseRegistrationAddDTO
            {
                StudentId = 1,
                CourseId = 1
            };

            var courseRegistrationAddDTO2 = new CourseRegistrationAddDTO
            {
                StudentId = 1,
                CourseId = 2
            };


            var result = await courseRegistrationService.AddCourse(courseRegistrationAddDTO);
            await courseRegistrationService.AddCourse(courseRegistrationAddDTO2);
            await courseRegistrationService.AddCourse(new CourseRegistrationAddDTO
            {
                StudentId = 1,
                CourseId = 3
            });

            Assert.IsNotNull(result);
            Assert.That(result.StudentId, Is.EqualTo(1));
            Assert.That(result.CourseId, Is.EqualTo(1));

            Assert.ThrowsAsync<StudentAlreadyRegisteredForCourseException>(async () => await courseRegistrationService.AddCourse(new CourseRegistrationAddDTO
            {
                StudentId = 1,
                CourseId = 3,
            }));
        }

        [Test, Order(2)]
        public async Task GetAllCourseRegistrationsSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.GetAllCourseRegistrations();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(3)]
        public async Task GetCourseRegistrationByIdSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.GetCourseRegistrationById(1);

            Assert.IsNotNull(result);
            Assert.That(result.RegistrationId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task UpdateCourseRegistrationSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.UpdateCourseRegistraion(1, 4);
            Assert.IsNotNull(result);
            Assert.That(result.CourseId, Is.EqualTo(4));

            //Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationService.UpdateCourseRegistraion(1000, 4));

            //Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseRegistrationService.UpdateCourseRegistraion(1, 1000));
        }

        [Test, Order(5)]
        public async Task GetCoursesRegisteredByStudentSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.GetCoursesRegisteredByStudent(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(6)]
        public async Task GetRegisteredStudentsForCourseSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.GetRegisteredStudentsForCourse(2);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(7)]
        public async Task ApproveCourseRegistrationsSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.ApproveCourseRegistrations(1);

            Assert.ThrowsAsync<CourseRegistrationAlreadyApprovedException>(async () => await courseRegistrationService.ApproveCourseRegistrations(1));

            Assert.IsNotNull(result);
        }



        [Test, Order(8)]
        public async Task DeleteCourseRegistrationSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.DeleteCourseRegistration(2);

            Assert.IsNotNull(result);
            Assert.That(result.RegistrationId, Is.EqualTo(2));
        }

        [Test, Order(9)]
        public async Task ApproveCourseRegistrationsForStudentSuccess()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            var result = await courseRegistrationService.ApproveCourseRegistrationsForStudent(1);

            Assert.ThrowsAsync<CourseRegistrationAlreadyApprovedException>(async () => await courseRegistrationService.ApproveCourseRegistrations(1));

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        #endregion

        #region Failure Tests
        [Test, Order(10)]
        public void AddCourseRegistrationFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo,_mapper, mockLoggerConfig.Object);

            var courseRegistrationAddDTO = new CourseRegistrationAddDTO
            {
                StudentId = 99, // Non-existent Student ID
                CourseId = 1
            };

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await courseRegistrationService.AddCourse(courseRegistrationAddDTO));

            courseRegistrationAddDTO = new CourseRegistrationAddDTO
            {
                StudentId = 1, 
                CourseId = 55// Non-existent Course ID
            };

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseRegistrationService.AddCourse(courseRegistrationAddDTO));

            courseRegistrationAddDTO = new CourseRegistrationAddDTO
            {
                StudentId = 1, // Non-existent Student ID
                CourseId = 3
            };

            Assert.ThrowsAsync<StudentAlreadyRegisteredForCourseException>(async () => await courseRegistrationService.AddCourse(courseRegistrationAddDTO));



        }

        [Test, Order(11)]
        public async Task GetAllCourseRegistrationsFailure()
        {
            await ClearDatabase();
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoCourseRegistrationsExistsException>(async () => await courseRegistrationService.GetAllCourseRegistrations());
        }

        [Test, Order(12)]
        public void GetCourseRegistrationByIdFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationService.GetCourseRegistrationById(99)); // Non-existent ID
        }

        [Test, Order(13)]
        public void UpdateCourseRegistrationFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationService.UpdateCourseRegistraion(99, 2)); // Non-existent course registration ID
        }

        [Test, Order(14)]
        public void DeleteCourseRegistrationFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationService.DeleteCourseRegistration(99)); // Non-existent course registration ID
        }

        [Test, Order(15)]
        public void GetCoursesRegisteredByStudentFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await courseRegistrationService.GetCoursesRegisteredByStudent(99)); // Non-existent student ID

        }

        [Test, Order(16)]
        public void GetRegisteredStudentsForCourseFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseRegistrationService.GetRegisteredStudentsForCourse(99)); // Non-existent course ID
        }

        [Test, Order(17)]
        public void ApproveCourseRegistrationsFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationService.ApproveCourseRegistrations(99)); // Non-existent course registration ID
        }

        [Test, Order(18)]
        public void ApproveCourseRegistrationsForStudentFailure()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await courseRegistrationService.ApproveCourseRegistrationsForStudent(99)); // Non-existent student ID
        }

        [Test, Order(19)]
        public async Task AddCourseRegistrationFailureTests()
        {
            ICourseRegistrationService courseRegistrationService = new CourseRegistrationService(_courseRegistrationRepo, _courseRepo, _studentRepo, _mapper, mockLoggerConfig.Object);
            var hmac = new HMACSHA512();
            var student1 = new Student
            {
                Name = "student5",
                Email = "student5@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("student5")),
                DepartmentId = 1,
                EWallet = 1000
            };

            await context.Students.AddRangeAsync(student1);

            var course1 = new Course
            {
                Name = "REACT",
                Description = "This is a REACT Course",
                FacultyId = 1,
                CourseVacancy = 1,
                CourseFees = 2000,
            };

            await context.Courses.AddRangeAsync(course1);

            await context.SaveChangesAsync();


            Assert.ThrowsAsync<InsufficientWallentBalanceException>(async () => await courseRegistrationService.AddCourse(new CourseRegistrationAddDTO { 
                
                StudentId = 2,
                CourseId = 5,
                
            }));

            var course6 = new Course
            {
                Name = "REACT",
                Description = "This is a REACT Course",
                FacultyId = 1,
                CourseVacancy = 0,
                CourseFees = 80,
            };

            await context.Courses.AddRangeAsync(course6);

            await context.SaveChangesAsync();

            Assert.ThrowsAsync<InsufficientVacancyException>(async () => await courseRegistrationService.AddCourse(new CourseRegistrationAddDTO
            {

                StudentId = 2,
                CourseId = 6,

            }));

        }

        #endregion
    }
}
