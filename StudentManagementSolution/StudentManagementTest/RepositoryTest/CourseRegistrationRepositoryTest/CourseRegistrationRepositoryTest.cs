using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions;
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
using System.Xml.Linq;

namespace StudentManagementTest.RepositoryTest.CourseRegistrationRepositoryTest
{
    public class CourseRegistrationRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, CourseRegistration> courseRegistrationRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyCourseRegistrationRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            courseRegistrationRepo = new CourseRegistrationRepository(context);
        }

        [Test, Order(1)]
        public async Task AddCourseRegistrationSuccess()
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

            var courseRegistration = new CourseRegistration
            {
                StudentId = student.StudentRollNo,
                CourseId = course.CourseId,
                Comments = "Approved!"
            };

            var addedCourseRegistration = await courseRegistrationRepo.Add(courseRegistration);
            Assert.That(addedCourseRegistration.StudentId, Is.EqualTo(courseRegistration.StudentId));
        }

        [Test, Order(2)]
        public async Task GetAllCourseRegistrationsSuccess()
        {
            var courseRegistrations = await courseRegistrationRepo.GetAll();
            Assert.That(courseRegistrations.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetCourseRegistrationByIdSuccess()
        {
            var courseRegistration = await courseRegistrationRepo.GetById(1);
            Assert.That(courseRegistration.StudentId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task UpdateCourseRegistrationSuccess()
        {
            var course = new Course
            {
                Name = "Introduction to JAVA",
                Description = "This is a Introduction to JAVA",
                FacultyId = 1, 
            };
            await context.Courses.AddRangeAsync(course);
            await context.SaveChangesAsync();

            var courseRegistration = await courseRegistrationRepo.GetById(1);
            courseRegistration.CourseId = 2;
            var updatedCourseRegistration = await courseRegistrationRepo.Update(courseRegistration);
            Assert.That(updatedCourseRegistration.CourseId, Is.EqualTo(2));
        }

        [Test, Order(5)]
        public async Task DeleteCourseRegistrationSuccess()
        {
            var courseRegistration = await courseRegistrationRepo.Delete(1);
            Assert.That(courseRegistration.StudentId, Is.EqualTo(1));
            Assert.IsEmpty(await courseRegistrationRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddCourseRegistrationFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await courseRegistrationRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllCourseRegistrationsFailure()
        {
            var courseRegistrations = await courseRegistrationRepo.GetAll();
            Assert.IsEmpty(courseRegistrations);
        }

        [Test, Order(8)]
        public void GetCourseRegistrationByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationRepo.GetById(1));
        }

        [Test, Order(9)]
        public async Task UpdateCourseRegistrationFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await courseRegistrationRepo.Update(null));
        }

        [Test, Order(10)]
        public async Task DeleteCourseRegistrationFailure()
        {
            Assert.ThrowsAsync<NoSuchCourseRegistrationExistException>(async () => await courseRegistrationRepo.Delete(1));
        }
    }

}
