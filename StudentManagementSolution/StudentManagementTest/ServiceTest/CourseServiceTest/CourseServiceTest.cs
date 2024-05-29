using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.CourseDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services.Course_Service;
using System.Security.Cryptography;
using System.Text;


namespace StudentManagementTest.ServiceTest.CourseServiceTest
{
    public class CourseServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Course> _courseRepo;
        IRepository<int, Faculty> _facultyRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        Mock<ILogger<CourseService>> mockLoggerConfig;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyCourseServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _courseRepo = new CourseRepository(context);
            _facultyRepo = new FacultyRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "StudentManagementApplicationAPI"
            }));
            _mapper = _config.CreateMapper();

            mockLoggerConfig = new Mock<ILogger<CourseService>>();
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

            var course1 = new Course
            {
                Name = "C#",
                Description = "This is a C# Course",
                FacultyId = 1
            };
            var course2 = new Course
            {
                Name = "MERN",
                Description = "This is a MERN Course",
                FacultyId = 1
            };
            await context.Courses.AddRangeAsync(course1, course2);
            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Students.RemoveRange(context.Students);
            context.Departments.RemoveRange(context.Departments);
            context.Faculties.RemoveRange(context.Faculties);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Success Test
        [Test, Order(1)]
        public async Task AddCourseSuccess()
        {
            await SeedDatabaseAsync();
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            var courseDTO = new CourseDTO
            {
                Name = "New Course",
                Description = "This is a new course",
                FacultyId = 1
            };

            var result = await courseService.AddCourse(courseDTO);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(courseDTO.Name));
            Assert.That(result.Description, Is.EqualTo(courseDTO.Description));
            Assert.That(result.FacultyId, Is.EqualTo(courseDTO.FacultyId));
        }

        [Test, Order(2)]
        public async Task GetAllCoursesSuccess()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            var result = await courseService.GetAllCourses();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test, Order(3)]
        public async Task GetCourseByIdSuccess()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);
            await SeedDatabaseAsync();

            var result = await courseService.GetCourseById(1);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("C#"));
        }

        [Test, Order(4)]
        public async Task GetCourseByNameSuccess()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);
            await SeedDatabaseAsync();

            var result = await courseService.GetCourseByName("C#");

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.First().Name, Is.EqualTo("C#"));
        }

        [Test, Order(5)]
        public async Task GetCourseByFacultySuccess()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);
            await SeedDatabaseAsync();

            var result = await courseService.GetCourseByFaculty(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.First().FacultyId, Is.EqualTo(1));
        }

        [Test, Order(6)]
        public async Task UpdateCourseSuccess()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);
            await SeedDatabaseAsync();

            var courseDTO = new CourseDTO
            {
                Name = "Updated Course",
                Description = "This is an updated course",
                FacultyId = 1
            };

            var result = await courseService.UpdateCourse(1, courseDTO);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(courseDTO.Name));
            Assert.That(result.Description, Is.EqualTo(courseDTO.Description));
            Assert.That(result.FacultyId, Is.EqualTo(courseDTO.FacultyId));
        }

        #endregion

        #region Failure Test
        [Test, Order(7)]
        public void AddCourseFailure()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            var courseDTO = new CourseDTO
            {
                Name = "C#",
                Description = "This is a new course",
                FacultyId = 1
            }; // Invalid Name

            Assert.ThrowsAsync<CourseAlreadyExistsException>(async () => await courseService.AddCourse(courseDTO));

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await courseService.AddCourse(new CourseDTO
            {
                Name = "Newly added course",
                Description = "This is a new course",
                FacultyId = 100
            }));
        }

        [Test, Order(8)]
        public void GetCourseByIdFailure()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseService.GetCourseById(99)); // Non-existent ID
        }

        [Test, Order(9)]
        public void GetCourseByNameFailure()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseService.GetCourseByName("NonExistentCourse"));
        }

        [Test, Order(10)]
        public void GetCourseByFacultyFailure()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoCoursesExistForFacultyException>(async () => await courseService.GetCourseByFaculty(99)); // Non-existent faculty ID
        }

        [Test, Order(11)]
        public void UpdateCourseFailure()
        {
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            var courseDTO = new CourseDTO
            {
                Name = "Updated Course",
                Description = "This is an updated course",
                FacultyId = 1
            };

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseService.UpdateCourse(99, courseDTO)); // Non-existent course ID
        }


        [Test, Order(12)]
        public async Task GetAllCoursesFailure()
        {
            await ClearDatabase();
            ICourseService courseService = new CourseService(_courseRepo, _mapper, _facultyRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoCoursesExistsException>(async () => await courseService.GetAllCourses());

        }

        #endregion
    }
}
