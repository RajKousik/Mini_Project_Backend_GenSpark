using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
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

namespace StudentManagementTest.RepositoryTest.CourseRepositoryTest
{
    public class CourseRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, Course> courseRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyCourseRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            courseRepo = new CourseRepository(context);
        }

        [Test, Order(1)]
        public async Task AddCourseSuccess()
        {
            var department1 = new Department { Name = "Computer Science", HeadId = 1 };
            await context.Departments.AddRangeAsync(department1);
            await context.SaveChangesAsync();

            var hmac = new HMACSHA512();
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
                FacultyId = 1
            };
            var addedCourse = await courseRepo.Add(course);
            Assert.That(addedCourse.Name, Is.EqualTo(course.Name));
        }

        [Test, Order(2)]
        public async Task GetAllCoursesSuccess()
        {
            var courses = await courseRepo.GetAll();
            Assert.That(courses.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetCourseByIdSuccess()
        {
            var course = await courseRepo.GetById(1);
            Assert.That(course.Name, Is.EqualTo("Introduction to Programming"));
        }

        [Test, Order(4)]
        public async Task UpdateCourseSuccess()
        {
            var course = await courseRepo.GetById(1);
            course.Name = "Advanced Programming";
            var updatedCourse = await courseRepo.Update(course);
            Assert.That(updatedCourse.Name, Is.EqualTo("Advanced Programming"));
        }

        [Test, Order(5)]
        public async Task DeleteCourseSuccess()
        {
            var course = await courseRepo.Delete(1);
            Assert.That(course.Name, Is.EqualTo("Advanced Programming"));
            Assert.IsEmpty(await courseRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddCourseFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await courseRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllCoursesFailure()
        {
            var courses = await courseRepo.GetAll();
            Assert.IsEmpty(courses);
        }

        [Test, Order(8)]
        public void GetCourseByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseRepo.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateCourseFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await courseRepo.Update(null));
        }

        [Test, Order(10)]
        public void DeleteCourseFailure()
        {
            Assert.ThrowsAsync<NoSuchCourseExistException>(async () => await courseRepo.Delete(1));
        }
    }

}
