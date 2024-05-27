using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions;
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

namespace StudentManagementTest.RepositoryTest.StudentAttendanceRepositoryTest
{
    public class StudentAttendanceRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, StudentAttendance> studentAttendanceRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyStudentAttendanceRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            studentAttendanceRepo = new StudentAttendanceRepository(context);
        }

        [Test, Order(1)]
        public async Task AddStudentAttendanceSuccess()
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

            var studentAttendance = new StudentAttendance
            {
                StudentRollNo = student.StudentRollNo,
                CourseId = course.CourseId,
                Date = DateTime.Now.Date,
                AttendanceStatus = AttendanceStatus.Present
            };

            var addedStudentAttendance = await studentAttendanceRepo.Add(studentAttendance);
            Assert.That(addedStudentAttendance.AttendanceStatus, Is.EqualTo(studentAttendance.AttendanceStatus));
        }

        [Test, Order(2)]
        public async Task GetAllStudentAttendancesSuccess()
        {
            var studentAttendances = await studentAttendanceRepo.GetAll();
            Assert.That(studentAttendances.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetStudentAttendanceByIdSuccess()
        {
            var studentAttendance = await studentAttendanceRepo.GetById(1);
            Assert.That(studentAttendance.AttendanceStatus, Is.EqualTo(AttendanceStatus.Present));
        }

        [Test, Order(4)]
        public async Task UpdateStudentAttendanceSuccess()
        {
            var studentAttendance = await studentAttendanceRepo.GetById(1);
            studentAttendance.AttendanceStatus = AttendanceStatus.Absent;
            var updatedStudentAttendance = await studentAttendanceRepo.Update(studentAttendance);
            Assert.That(updatedStudentAttendance.AttendanceStatus, Is.EqualTo(AttendanceStatus.Absent));
        }

        [Test, Order(5)]
        public async Task DeleteStudentAttendanceSuccess()
        {
            var studentAttendance = await studentAttendanceRepo.Delete(1);
            Assert.That(studentAttendance.AttendanceStatus, Is.EqualTo(AttendanceStatus.Absent));
            Assert.IsEmpty(await studentAttendanceRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddStudentAttendanceFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await studentAttendanceRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllStudentAttendancesFailure()
        {
            var studentAttendances = await studentAttendanceRepo.GetAll();
            Assert.IsEmpty(studentAttendances);
        }

        [Test, Order(8)]
        public void GetStudentAttendanceByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchStudentAttendanceExistException>(async () => await studentAttendanceRepo.GetById(1));
        }

        [Test, Order(9)]
        public async Task UpdateStudentAttendanceFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await studentAttendanceRepo.Update(null));
        }

        [Test, Order(10)]
        public async Task DeleteStudentAttendanceFailure()
        {
            Assert.ThrowsAsync<NoSuchStudentAttendanceExistException>(async () => await studentAttendanceRepo.Delete(1));
        }
    }

}
