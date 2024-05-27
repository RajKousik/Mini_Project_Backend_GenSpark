using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTest.RepositoryTest.ExamRepositoryTest
{
    public class ExamRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, Exam> examRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyExamRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            examRepo = new ExamRepository(context);
        }

        [Test, Order(1)]
        public async Task AddExamSuccess()
        {
            var department = new Department { Name = "Computer Science", HeadId = 1 };
            await context.Departments.AddRangeAsync(department);
            var hmac = new HMACSHA512();
            var student = new Student
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
            var faculty1 = new Faculty
            {
                Name = "faculty1",
                Email = "faculty1@gmail.com",
                DOB = new DateTime(1980, 01, 01),
                Gender = "Female",
                Address = "New York",
                Mobile = "1234567890",
                Role = RoleType.Admin,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1
            };
            await context.Faculties.AddRangeAsync(faculty1);
            await context.SaveChangesAsync();
            var course = new Course
            {
                Name = "Introduction to Programming",
                Description = "This is a Introduction to Programming",
                FacultyId = 1,
            };
            await context.Students.AddRangeAsync(student);
            await context.Courses.AddRangeAsync(course);
            await context.SaveChangesAsync();

            var exam = new Exam
            {
                CourseId = course.CourseId,
                ExamDate = DateTime.Now.AddDays(7),
                TotalMark = 100,
                ExamType = ExamType.Offline,
                StartTime = DateTime.Now.AddDays(7).AddHours(10),
                EndTime = DateTime.Now.AddDays(7).AddHours(12),
            };

            var addedExam = await examRepo.Add(exam);
            Assert.That(addedExam.CourseId, Is.EqualTo(exam.CourseId));
        }

        [Test, Order(2)]
        public async Task GetAllExamsSuccess()
        {
            var exams = await examRepo.GetAll();
            Assert.That(exams.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetExamByIdSuccess()
        {
            var exam = await examRepo.GetById(1);
            Assert.That(exam.CourseId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task UpdateExamSuccess()
        {
            var exam = await examRepo.GetById(1);
            exam.TotalMark = 80;
            var updatedExam = await examRepo.Update(exam);
            Assert.That(updatedExam.TotalMark, Is.EqualTo(80));
        }

        [Test, Order(5)]
        public async Task DeleteExamSuccess()
        {
            var exam = await examRepo.Delete(1);
            Assert.That(exam.TotalMark, Is.EqualTo(80));
            Assert.IsEmpty(await examRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddExamFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await examRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllExamsFailure()
        {
            var exams = await examRepo.GetAll();
            Assert.IsEmpty(exams);
        }

        [Test, Order(8)]
        public void GetExamByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examRepo.GetById(1));
        }

        [Test, Order(9)]
        public async Task UpdateExamFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await examRepo.Update(null));
        }

        [Test, Order(10)]
        public async Task DeleteExamFailure()
        {
            Assert.ThrowsAsync<NoSuchExamExistException>(async () => await examRepo.Delete(1));
        }
    }

}
