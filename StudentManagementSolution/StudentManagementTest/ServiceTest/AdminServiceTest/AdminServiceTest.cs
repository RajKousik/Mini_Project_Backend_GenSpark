using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service.AdminService;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTest.ServiceTest.AdminServiceTest
{
    public class AdminServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Student> _studentRepo;
        IRepository<int, Faculty> _facultyRepo;
        Mock<ILogger<AdminService>> mockLoggerConfig;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyAdminServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _studentRepo = new StudentRepository(context);
            _facultyRepo = new FacultyRepository(context);

            mockLoggerConfig = new Mock<ILogger<AdminService>>();

        }

        private async Task SeedDatabaseAsync()
        {
            var hmac = new HMACSHA512();
            var faculty = new Faculty
            {
                Name = "faculty1",
                Email = "faculty1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Proffesors,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };

            var department1 = new Department { Name = "Computer Science", HeadId = 1 };

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

            await context.Departments.AddRangeAsync(department1);

            await context.Students.AddRangeAsync(student);
            await context.Faculties.AddRangeAsync(faculty);
            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Students.RemoveRange(context.Students);
            context.Faculties.RemoveRange(context.Faculties);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Success Tests

        [Test, Order(1)]
        public async Task ActivateStudentSuccess()
        {
            await SeedDatabaseAsync();
            IAdminService adminService = new AdminService(_studentRepo, _facultyRepo, mockLoggerConfig.Object);

            var result = await adminService.ActivateStudent("student1@gmail.com");

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo("Student account activated successfully"));
        }

        [Test, Order(2)]
        public async Task ActivateFacultySuccess()
        {
            await SeedDatabaseAsync();
            IAdminService adminService = new AdminService(_studentRepo, _facultyRepo, mockLoggerConfig.Object);

            var result = await adminService.ActivateFaculty("faculty1@gmail.com");

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo("Faculty account activated successfully"));
        }

        #endregion

        #region Failure Tests

        [Test, Order(3)]
        public async Task ActivateStudentFailure_InvalidEmail()
        {
            IAdminService adminService = new AdminService(_studentRepo, _facultyRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await adminService.ActivateStudent("invalidemail@example.com"));

        }

        [Test, Order(4)]
        public void ActivateFacultyFailure_InvalidEmail()
        {
            IAdminService adminService = new AdminService(_studentRepo, _facultyRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<FacultyAlreadyActivatedException>(async () => await adminService.ActivateFaculty("faculty1@gmail.com"));

        }

        #endregion
    }

}
