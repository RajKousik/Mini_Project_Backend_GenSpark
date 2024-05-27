using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
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

namespace StudentManagementTest.RepositoryTest.DepartmentRepositoryTest
{
    public class DepartmentRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, Department> departmentRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyDepartmentRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            departmentRepo = new DepartmentRepository(context);
        }

        [Test, Order(1)]
        public async Task AddDepartmentSuccess()
        {
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
            var department = new Department
            {
                Name = "Computer Science",
                HeadId = 1
            };

            var addedDepartment = await departmentRepo.Add(department);
            Assert.That(addedDepartment.Name, Is.EqualTo(department.Name));
        }

        [Test, Order(2)]
        public async Task GetAllDepartmentsSuccess()
        {
            var departments = await departmentRepo.GetAll();
            Assert.That(departments.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetDepartmentByIdSuccess()
        {
            var department = await departmentRepo.GetById(1);
            Assert.That(department.Name, Is.EqualTo("Computer Science"));
        }

        [Test, Order(4)]
        public async Task UpdateDepartmentSuccess()
        {
            var department = await departmentRepo.GetById(1);
            department.Name = "Updated Computer Science";
            var updatedDepartment = await departmentRepo.Update(department);
            Assert.That(updatedDepartment.Name, Is.EqualTo("Updated Computer Science"));
        }

        [Test, Order(5)]
        public async Task DeleteDepartmentSuccess()
        {
            var department = await departmentRepo.Delete(1);
            Assert.That(department.Name, Is.EqualTo("Updated Computer Science"));
            Assert.IsEmpty(await departmentRepo.GetAll());
        }

        [Test, Order(6)]
        public void AddDepartmentFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await departmentRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllDepartmentsFailure()
        {
            var departments = await departmentRepo.GetAll();
            Assert.IsEmpty(departments);
        }

        [Test, Order(8)]
        public void GetDepartmentByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchDepartmentExistException>(async () => await departmentRepo.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateDepartmentFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await departmentRepo.Update(null));
        }

        [Test, Order(10)]
        public void DeleteDepartmentFailure()
        {
            Assert.ThrowsAsync<NoSuchDepartmentExistException>(async () => await departmentRepo.Delete(1));
        }
    }

}
