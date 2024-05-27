using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
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

namespace StudentManagementTest.RepositoryTest.FacultyRepositoryTest
{
    public class FacultyRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, Faculty> facultyRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyFacultyRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            facultyRepo = new FacultyRepository(context);
        }

        [Test, Order(1)]
        public async Task AddFacultySuccess()
        {
            var department1 = new Department { Name = "Computer Science", HeadId = 1 };

            await context.Departments.AddRangeAsync(department1);
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
            var addedFaculty = await facultyRepo.Add(faculty1);
            Assert.That(addedFaculty.Email, Is.EqualTo(faculty1.Email));
        }

        [Test, Order(2)]
        public async Task GetAllFacultiesSuccess()
        {
            var faculties = await facultyRepo.GetAll();
            Assert.That(faculties.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetFacultyByIdSuccess()
        {
            var faculty = await facultyRepo.GetById(1);
            Assert.That(faculty.Email, Is.EqualTo("faculty1@gmail.com"));
        }

        [Test, Order(4)]
        public async Task UpdateFacultySuccess()
        {
            var faculty = await facultyRepo.GetById(1);
            faculty.Email = "updated@gmail.com";
            var updatedFaculty = await facultyRepo.Update(faculty);
            Assert.That(updatedFaculty.Email, Is.EqualTo("updated@gmail.com"));
        }

        [Test, Order(5)]
        public async Task DeleteFacultySuccess()
        {
            var faculty = await facultyRepo.Delete(1);
            Assert.That(faculty.Email, Is.EqualTo("updated@gmail.com"));
            Assert.IsEmpty(await facultyRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddFacultyFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await facultyRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllFacultiesFailure()
        {
            var faculties = await facultyRepo.GetAll();
            Assert.IsEmpty(faculties);
        }

        [Test, Order(8)]
        public void GetFacultyByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyRepo.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateFacultyFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await facultyRepo.Update(null));
        }

        [Test, Order(10)]
        public void DeleteFacultyFailure()
        {
            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyRepo.Delete(1));
        }
    }

}
