using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
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

namespace StudentManagementTest.RepositoryTest.GradeRepositoryTest
{
    public class GradeRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, Grade> gradeRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyGradeRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            gradeRepo = new GradeRepository(context);
        }

        [Test, Order(1)]
        public async Task AddGradeSuccess()
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

            var faculty = new Faculty
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

            await context.Faculties.AddRangeAsync(faculty);
            await context.Students.AddRangeAsync(student);
            await context.SaveChangesAsync();

            var course = new Course
            {
                Name = "Introduction to Programming",
                Description = "This is an Introduction to Programming",
                FacultyId = faculty.FacultyId,
            };

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

            await context.Exams.AddRangeAsync(exam);
            await context.SaveChangesAsync();

            var grade = new Grade
            {
                ExamId = exam.ExamId,
                StudentId = 1,
                MarksScored = 90,
                EvaluatedById = 1,
                Percentage = 90,
                StudentGrade = GradeType.A_Plus,
                Comments = "Good!"
            };

            var addedGrade = await gradeRepo.Add(grade);
            Assert.That(addedGrade.MarksScored, Is.EqualTo(grade.MarksScored));
        }

        [Test, Order(2)]
        public async Task GetAllGradesSuccess()
        {
            var grades = await gradeRepo.GetAll();
            Assert.That(grades.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetGradeByIdSuccess()
        {
            var grade = await gradeRepo.GetById(1);
            Assert.That(grade.MarksScored, Is.EqualTo(90));
        }

        [Test, Order(4)]
        public async Task UpdateGradeSuccess()
        {
            var grade = await gradeRepo.GetById(1);
            grade.MarksScored = 95;
            var updatedGrade = await gradeRepo.Update(grade);
            Assert.That(updatedGrade.MarksScored, Is.EqualTo(95));
        }

        [Test, Order(5)]
        public async Task DeleteGradeSuccess()
        {
            var grade = await gradeRepo.Delete(1);
            Assert.That(grade.MarksScored, Is.EqualTo(95));
            Assert.IsEmpty(await gradeRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddGradeFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await gradeRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllGradesFailure()
        {
            var grades = await gradeRepo.GetAll();
            Assert.IsEmpty(grades);
        }

        [Test, Order(8)]
        public void GetGradeByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchGradeRecordExistsException>(async () => await gradeRepo.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateGradeFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await gradeRepo.Update(null));
        }

        [Test, Order(10)]
        public void DeleteGradeFailure()
        {
            Assert.ThrowsAsync<NoSuchGradeRecordExistsException>(async () => await gradeRepo.Delete(1));
        }
    }

}
