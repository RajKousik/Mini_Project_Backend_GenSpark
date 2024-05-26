using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.ExamDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTest.ServiceTest.ExamServiceTest
{
    public class ExamServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Exam> _examRepo;
        IRepository<int, Course> _courseRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        #endregion

        #region Setup
        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyExamServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _examRepo = new ExamRepository(context);
            _courseRepo = new CourseRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
            "StudentManagementApplicationAPI"
        }));
            _mapper = _config.CreateMapper();
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
                DepartmentId = 1
            };

            await context.Students.AddRangeAsync(student1);

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
            var course3 = new Course
            {
                Name = "JAVA",
                Description = "This is a JAVA Course",
                FacultyId = 1
            };
            var course4 = new Course
            {
                Name = "CPP",
                Description = "This is a CPP Course",
                FacultyId = 1
            };
            await context.Courses.AddRangeAsync(course1, course2, course3, course4);
            await context.SaveChangesAsync();

            var exam1 = new Exam
            {
                CourseId = 1,
                TotalMark = 100,
                ExamDate = new DateTime(2024, 6, 1),
                StartTime = new DateTime(2024, 6, 1, 10, 0, 0),
                EndTime = new DateTime(2024, 6, 1, 12, 0, 0),
                ExamType = ExamType.Online
            };

            var exam2 = new Exam
            {
                CourseId = 2,
                TotalMark = 100,
                ExamDate = new DateTime(2024, 7, 1),
                StartTime = new DateTime(2024, 7, 1, 10, 0, 0),
                EndTime = new DateTime(2024, 7, 1, 12, 0, 0),
                ExamType = ExamType.Offline
            };

            var exam3 = new Exam
            {
                CourseId = 3,
                TotalMark = 100,
                ExamDate = new DateTime(2024, 8, 1),
                StartTime = new DateTime(2024, 8, 1, 10, 0, 0),
                EndTime = new DateTime(2024, 8, 1, 12, 0, 0),
                ExamType = ExamType.Online
            };

            await context.Exams.AddRangeAsync(exam1, exam2, exam3);
            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Exams.RemoveRange(context.Exams);
            context.Faculties.RemoveRange(context.Faculties);
            context.Students.RemoveRange(context.Students);
            context.Courses.RemoveRange(context.Courses);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Success Tests

        [Test, Order(1)]
        public async Task AddExamSuccess()
        {
            await SeedDatabaseAsync();
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var examDTO = new ExamDTO
            {
                CourseId = 4,
                TotalMark = 100,
                ExamDate = new DateOnly(2024, 8, 1),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(12, 0, 0),
                ExamType = "Online"
            };

            var result = await examService.AddExam(examDTO);

            Assert.IsNotNull(result);
            Assert.That(result.TotalMark, Is.EqualTo(examDTO.TotalMark));
        }

        [Test, Order(2)]
        public async Task GetAllExamsSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var result = await examService.GetAllExams();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(3)]
        public async Task GetExamByIdSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var result = await examService.GetExamById(1);

            Assert.IsNotNull(result);
            Assert.That(result.ExamId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task UpdateExamSuccess()
        {
            IExamService examService = new ExamService(_examRepo,_courseRepo,_mapper);

            var examDTO = new ExamDTO
            {
                CourseId = 4,
                TotalMark = 100,
                ExamDate = new DateOnly(2024, 8, 1),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(12, 0, 0),
                ExamType = "Offline"
            };

            var result = await examService.UpdateExam(4, examDTO);

            Assert.IsNotNull(result);
            Assert.That(result.ExamType, Is.EqualTo("Offline"));
        }

        [Test, Order(5)]
        public async Task DeleteExamSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var result = await examService.DeleteExam(2);

            Assert.IsNotNull(result);
            Assert.That(result.ExamId, Is.EqualTo(2));
        }

        [Test, Order(6)]
        public async Task GetExamsByDateSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo,  _mapper);

            var result = await examService.GetExamsByDate(new DateTime(2024, 06, 01));

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(7)]
        public async Task GetUpcomingExamsSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var result = await examService.GetUpcomingExams(30);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(8)]
        public async Task GetOfflineExamsSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var result = await examService.GetOfflineExams();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(9)]
        public async Task GetOnlineExamsSuccess()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var result = await examService.GetOnlineExams();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        #endregion

        #region Failure Tests

        [Test, Order(10)]
        public async Task AddExamFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var examDTO = new ExamDTO
            {
                CourseId = 1,
                TotalMark = 100,
                ExamDate = new DateOnly(2024, 8, 1),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(12, 0, 0),
                ExamType = "Online"
            };

            Assert.ThrowsAsync<ExamAlreadyScheduledException>(async () => await examService.AddExam(examDTO));
        }

        [Test, Order(11)]
        public async Task GetAllExamsFailure()
        {
            await ClearDatabase();
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            Assert.ThrowsAsync<NoExamsExistsException>(async () => await examService.GetAllExams());
        }

        [Test, Order(12)]
        public async Task GetExamByIdFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examService.GetExamById(99)); // Non-existent ID
        }

        [Test, Order(13)]
        public async Task UpdateExamFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            var examDTO = new ExamDTO
            {
                CourseId = 4,
                TotalMark = 100,
                ExamDate = new DateOnly(2024, 8, 1),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(12, 0, 0),
                ExamType = "Online"
            };

            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examService.UpdateExam(99, examDTO)); // Non-existent ID
        }

        [Test, Order(14)]
        public async Task DeleteExamFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examService.DeleteExam(99)); // Non-existent ID
        }

        [Test, Order(15)]
        public async Task GetExamsByDateFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            Assert.ThrowsAsync<NoExamsExistsException>(async () => await examService.GetExamsByDate(new DateTime(2025, 01, 01))); // Date with no exams

            //    Assert.IsEmpty(result);
        }

        [Test, Order(16)]
        public async Task GetUpcomingExamsFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examService.GetUpcomingExams(0)); // No upcoming exams within 0 days

        }

        [Test, Order(17)]
        public async Task GetOfflineExamsFailure()
        {
            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);


            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examService.GetOfflineExams());

        }

        [Test, Order(18)]
        public async Task GetOnlineExamsFailure()
        {

            IExamService examService = new ExamService(_examRepo, _courseRepo, _mapper);

            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examService.GetOnlineExams());
        }

        #endregion
    }

}
