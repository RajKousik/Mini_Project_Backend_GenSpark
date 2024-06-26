﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.GradeDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services.Exam_Service;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementTest.ServiceTest.GradeServiceTest
{
    public class GradeServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Grade> _gradeRepo;
        IRepository<int, Exam> _examRepo;
        IRepository<int, Student> _studentRepo;
        IRepository<int, CourseRegistration> _courseRegistrationRepo;
        IRepository<int, Faculty> _facultyRepo;
        IRepository<int, Course> _courseRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        Mock<ILogger<GradeService>> mockLoggerConfig;

        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyGradeServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _gradeRepo = new GradeRepository(context);
            _examRepo = new ExamRepository(context);
            _studentRepo = new StudentRepository(context);
            _courseRegistrationRepo = new CourseRegistrationRepository(context);
            _facultyRepo = new FacultyRepository(context);
            _courseRepo = new CourseRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "StudentManagementApplicationAPI"
            }));
            _mapper = _config.CreateMapper();

            mockLoggerConfig = new Mock<ILogger<GradeService>>();
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

            var course2 = new Course
            {
                Name = "Science",
                Description = "Science Course",
                FacultyId = 1
            };

            var courseRegistration1 = new CourseRegistration
            {
                CourseId = 1,
                StudentId = 1,
                ApprovalStatus = ApprovalStatus.Approved,
                Comments = "Approved!"
            };

            var courseRegistration2 = new CourseRegistration
            {
                CourseId = 2,
                StudentId = 1,
                ApprovalStatus = ApprovalStatus.Approved,
                Comments = "Approved!"
            };

            var exam1 = new Exam
            {
                CourseId = 1,
                TotalMark = 100,
                ExamDate = new DateTime(2024, 8, 1),
                StartTime = new DateTime(2024, 8, 1, 10, 0, 0),
                EndTime = new DateTime(2024, 8, 1, 12, 0, 0),
                ExamType = ExamType.Online
            };

            var exam2 = new Exam
            {
                CourseId = 2,
                TotalMark = 100,
                ExamDate = new DateTime(2024, 8, 1),
                StartTime = new DateTime(2024, 8, 1, 10, 0, 0),
                EndTime = new DateTime(2024, 8, 1, 12, 0, 0),
                ExamType = ExamType.Online
            };

            var grade1 = new Grade
            {
                StudentId = 1,
                ExamId = 1,
                EvaluatedById = 1,
                MarksScored = 80,
                Comments = "Good",
                Percentage = 80,
                StudentGrade = GradeType.A
            };

            await context.Students.AddRangeAsync(student1);
            await context.Faculties.AddRangeAsync(faculty1);
            await context.Departments.AddRangeAsync(department1);
            await context.Courses.AddRangeAsync(course1, course2);
            await context.Exams.AddRangeAsync(exam1, exam2);
            await context.Grades.AddRangeAsync(grade1);
            await context.CourseRegistrations.AddRangeAsync(courseRegistration1, courseRegistration2);
            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Students.RemoveRange(context.Students);
            context.Courses.RemoveRange(context.Courses);
            context.Exams.RemoveRange(context.Exams);
            context.Grades.RemoveRange(context.Grades);
            //context.Departments.RemoveRange(context.Departments);
            context.Faculties.RemoveRange(context.Faculties);
            context.CourseRegistrations.RemoveRange(context.CourseRegistrations);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Success Tests

        [Test, Order(1)]
        public async Task AddGradeSuccess()
        {
            await SeedDatabaseAsync();
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var gradeDTO = new GradeDTO
            {
                StudentId = 1,
                ExamId = 2,
                MarksScored = 90,
                EvaluatedById = 1,
                Comments = "Good"
            };

            var result = await gradeService.AddGrade(gradeDTO);
            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await gradeService.AddGrade(new GradeDTO
            {
                StudentId = 100,
                ExamId = 1,
                MarksScored = 90,
                EvaluatedById = 1,
                Comments = "Good"
            }));

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await gradeService.AddGrade(new GradeDTO
            {
                StudentId = 1,
                ExamId = 1,
                MarksScored = 90,
                EvaluatedById = 100,
                Comments = "Good"
            }));

            Assert.IsNotNull(result);
            Assert.That(result.MarksScored, Is.EqualTo(90));
        }

        [Test, Order(2)]
        public async Task GetAllGradesSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var result = await gradeService.GetAllGrades();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(3)]
        public async Task GetGradeByIdSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var result = await gradeService.GetGradeById(1);

            Assert.IsNotNull(result);
            Assert.That(result.MarksScored, Is.EqualTo(80));
        }

        [Test, Order(4)]
        public async Task UpdateGradeSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var gradeUpdateDTO = new GradeUpdateDTO
            {
                EvaluatedById = 1,
                MarksScored = 95,
                Comments = "Superb!"
            };

            var result = await gradeService.UpdateGrade(1, gradeUpdateDTO);

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await gradeService.UpdateGrade(1, gradeUpdateDTO = new GradeUpdateDTO
            {
                EvaluatedById = 100,
                MarksScored = 95,
                Comments = "Superb!"
            }));

            Assert.ThrowsAsync<InvalidMarksScoredException>(async () => await gradeService.UpdateGrade(1, gradeUpdateDTO = new GradeUpdateDTO
            {
                EvaluatedById = 1,
                MarksScored = 120,
                Comments = "Superb!"
            }));
            ;
            Assert.IsNotNull(result);
            Assert.That(result.MarksScored, Is.EqualTo(95));
        }

        [Test, Order(5)]
        public async Task GetStudentGradesSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var result = await gradeService.GetStudentGrades(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(6)]
        public async Task GetCourseGradesSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var result = await gradeService.GetCourseGrades(1);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test, Order(6)]
        public async Task GetTopStudentsSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var result = await gradeService.GetTopStudentsByCourse(1);

            Assert.IsNotNull(result);

        }

        [Test, Order(7)]
        public async Task DeleteGradeSuccess()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var result = await gradeService.DeleteGrade(1);

            Assert.IsNotNull(result);
            Assert.That(result.MarksScored, Is.EqualTo(95));
        }





        #endregion

        #region Failure Tests

        [Test, Order(8)]
        public void AddGradeFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var gradeDTO = new GradeDTO
            {
                StudentId = 99, // Non-existent student ID
                ExamId = 1,
                EvaluatedById = 1,
                MarksScored = 90,
                Comments = "Good!"
            };

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await gradeService.AddGrade(gradeDTO));

            gradeDTO = new GradeDTO
            {
                StudentId = 1, 
                ExamId = 200,// Non-existent Exam ID
                EvaluatedById = 1,
                MarksScored = 90,
                Comments = "Good!"
            };

            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await gradeService.AddGrade(gradeDTO));

            gradeDTO = new GradeDTO
            {
                StudentId = 1, 
                ExamId = 1,
                EvaluatedById = 1000, // Non-existent Evalauted by ID
                MarksScored = 79,
                Comments = "Good!"
            };

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await gradeService.AddGrade(gradeDTO));

            gradeDTO = new GradeDTO
            {
                StudentId = 1,
                ExamId = 1,
                EvaluatedById = 1, // Non-existent Evalauted by ID
                MarksScored = 120,
                Comments = "Good!"
            };

            Assert.ThrowsAsync<InvalidMarksScoredException>(async () => await gradeService.AddGrade(gradeDTO));
        }

        [Test, Order(9)]
        public async Task GetAllGradesFailure()
        {
            await ClearDatabase();
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoGradeRecordsExistsException>(async () => await gradeService.GetAllGrades());
        }

        [Test, Order(10)]
        public void GetGradeByIdFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchGradeRecordExistsException>(async () => await gradeService.GetGradeById(99)); // Non-existent grade ID
        }

        [Test, Order(11)]
        public void UpdateGradeFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            var gradeUpdateDTO = new GradeUpdateDTO
            {
                MarksScored = 100,
                Comments = "Superb!",
                EvaluatedById = 1
            };

            Assert.ThrowsAsync<NoSuchGradeRecordExistsException>(async () => await gradeService.UpdateGrade(99, gradeUpdateDTO)); // Non-existent grade ID

        }

        [Test, Order(12)]
        public void DeleteGradeFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchGradeRecordExistsException>(async () => await gradeService.DeleteGrade(99)); // Non-existent grade ID
        }

        [Test, Order(13)]
        public async Task GetStudentGradesFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await gradeService.GetStudentGrades(99)); // Non-existent student ID
            var hmac = new HMACSHA512();
            var student1 = new Student
            {
                Name = "student2",
                Email = "student2@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Status = ActivationStatus.Active,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("student2")),
                DepartmentId = 1
            };

            await context.Students.AddRangeAsync(student1);
            await context.SaveChangesAsync();

            Assert.ThrowsAsync<NoGradeRecordsExistsException>(async () => await gradeService.GetStudentGrades(2));

        }

        [Test, Order(14)]
        public void GetCourseGradesFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await gradeService.GetCourseGrades(99)); // Non-existent course ID

            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await gradeService.GetCourseGrades(1));
        }

        [Test, Order(15)]
        public void GetTopStudentsFailure()
        {
            IGradeService gradeService = new GradeService(_gradeRepo, _examRepo, _studentRepo, _courseRegistrationRepo, _facultyRepo, _courseRepo, _mapper, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchCourseExistException>(async ()=> await gradeService.GetTopStudentsByCourse(100));

            //Assert.IsNotNull(result);

        }

        #endregion
    }

}
