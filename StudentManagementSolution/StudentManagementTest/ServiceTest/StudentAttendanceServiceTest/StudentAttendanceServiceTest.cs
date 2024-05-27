using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentAttendanceDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System.Security.Cryptography;
using System.Text;


namespace StudentManagementTest.ServiceTest.StudentAttendanceServiceTest
{
    public class StudentAttendanceServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Student> _studentRepo;
        IRepository<int, Course> _courseRepo;
        IRepository<int, CourseRegistration> _courseRegistrationRepo;
        IRepository<int, StudentAttendance> _attendanceRepo;
        IMapper _mapper;
        MapperConfiguration _config;

        Mock<ILogger<StudentAttendanceService>> mockLoggerConfig;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyStudentAttendanceServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _studentRepo = new StudentRepository(context);
            _courseRepo = new CourseRepository(context);
            _courseRegistrationRepo = new CourseRegistrationRepository(context);
            _attendanceRepo = new StudentAttendanceRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
            "StudentManagementApplicationAPI"
        }));
            _mapper = _config.CreateMapper();

            mockLoggerConfig = new Mock<ILogger<StudentAttendanceService>>();
        }

        private async Task SeedDatabaseAsync()
        {
            var hmac = new HMACSHA512();
            var faculty1 = new Faculty
            {
                Name = "faculty1",
                Email = "faculty1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Proffesors,
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1
            };
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

            var department1 = new Department { HeadId = 1, Name = "IT" };

            var course1 = new Course
            {
                Name = "Math",
                Description = "Math Course",
                FacultyId = 1
            };

            var courseRegistration1 = new CourseRegistration
            {
                CourseId = 1,
                StudentId = 1,
                IsApproved = true,
                Comments = "Approved!"
            };

            var attendance1 = new StudentAttendance
            {
                StudentRollNo = 1,
                CourseId = 1,
                Date = new DateTime(2024, 8, 1),
                AttendanceStatus = AttendanceStatus.Present
            };

            await context.Students.AddRangeAsync(student1);
            await context.Faculties.AddRangeAsync(faculty1);
            await context.Departments.AddRangeAsync(department1);
            await context.Courses.AddRangeAsync(course1);
            await context.StudentAttendances.AddRangeAsync(attendance1);
            await context.CourseRegistrations.AddRangeAsync(courseRegistration1);
            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Students.RemoveRange(context.Students);
            context.Courses.RemoveRange(context.Courses);
            context.Departments.RemoveRange(context.Departments);
            context.Faculties.RemoveRange(context.Faculties);
            context.CourseRegistrations.RemoveRange(context.CourseRegistrations);
            context.StudentAttendances.RemoveRange(context.StudentAttendances);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Success Tests

        [Test, Order(1)]
        public async Task MarkAttendanceSuccess()
        {
            await SeedDatabaseAsync();
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var attendanceDTO = new AttendanceDTO
            {
                StudentRollNo = 1,
                CourseId = 1,
                Date = new DateOnly(2023, 8, 2),
                AttendanceStatus = "present"
            };

            var result = await attendanceService.MarkAttendance(attendanceDTO);

            Assert.IsNotNull(result);
            Assert.That(result.StudentRollNo, Is.EqualTo(1));
            Assert.That(result.AttendanceStatus, Is.EqualTo("Present"));
        }

        [Test, Order(2)]
        public async Task UpdateAttendanceSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var result = await attendanceService.UpdateAttendance(1, "Absent");

            Assert.IsNotNull(result);
            Assert.That(result.AttendanceStatus, Is.EqualTo("Absent"));
        }

        [Test, Order(3)]
        public async Task GetAttendanceSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var result = await attendanceService.GetAttendance(1);

            Assert.IsNotNull(result);
            Assert.That(result.StudentRollNo, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task GetAllAttendanceRecordsSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var result = await attendanceService.GetAllAttendanceRecords();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(5)]
        public async Task GetStudentAttendanceRecordsSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var result = await attendanceService.GetStudentAttendanceRecords(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(6)]
        public async Task GetCourseAttendanceRecordsSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo,mockLoggerConfig.Object);

            var result = await attendanceService.GetCourseAttendanceRecords(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(7)]
        public async Task GetStudentAttendancePercentageSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var result = await attendanceService.GetStudentAttendancePercentage(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.First().AttendancePercentage, Is.EqualTo(50));
        }

        [Test, Order(8)]
        public async Task DeleteAttendanceSuccess()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var result = await attendanceService.DeleteAttendance(1);

            Assert.IsNotNull(result);
            Assert.That(result.StudentRollNo, Is.EqualTo(1));
        }

        #endregion

        #region Failure Tests

        [Test, Order(9)]
        public async Task MarkAttendanceFailure()
        {
            await ClearDatabase();
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            var attendanceDTO = new AttendanceDTO
            {
                StudentRollNo = 99, // Non-existent student roll number
                CourseId = 1,
                Date = new DateOnly(2024, 8, 2),
                AttendanceStatus = "Present"
            };

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await attendanceService.MarkAttendance(attendanceDTO));
        }

        [Test, Order(10)]
        public async Task UpdateAttendanceFailure()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentAttendanceExistException>(async () => await attendanceService.UpdateAttendance(99, "Absent")); // Non-existent attendance ID
        }

        [Test, Order(11)]
        public async Task DeleteAttendanceFailure()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentAttendanceExistException>(async () => await attendanceService.DeleteAttendance(99)); // Non-existent attendance ID
        }

        [Test, Order(12)]
        public async Task GetAttendanceFailure()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentAttendanceExistException>(async () => await attendanceService.GetAttendance(99)); // Non-existent attendance ID
        }

        [Test, Order(13)]
        public async Task GetAllAttendanceRecordsFailure()
        {
            await ClearDatabase();
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoStudentAttendancesExistsException>(async () => await attendanceService.GetAllAttendanceRecords());
        }

        [Test, Order(14)]
        public async Task GetStudentAttendanceRecordsFailure()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await attendanceService.GetStudentAttendanceRecords(99)); // Non-existent student ID
        }

        [Test, Order(15)]
        public async Task GetCourseAttendanceRecordsFailure()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await attendanceService.GetCourseAttendanceRecords(99)); // Non-existent course ID
        }

        [Test, Order(16)]
        public async Task GetStudentAttendancePercentageFailure()
        {
            IStudentAttendanceService attendanceService = new StudentAttendanceService(_studentRepo, _courseRepo, _courseRegistrationRepo, _mapper, _attendanceRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await attendanceService.GetStudentAttendancePercentage(99)); // Non-existent student ID
        }

        #endregion

    }

}
