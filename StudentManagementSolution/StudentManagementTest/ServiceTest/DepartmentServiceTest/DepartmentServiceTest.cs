using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTest.ServiceTest.DepartmentServiceTest
{
    public class DepartmentServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Student> _studentRepo;
        IRepository<int, Department> _departmentRepo;
        IRepository<int, Faculty> _facultyRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        #endregion

        #region Setup
        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyDepartmentServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _studentRepo = new StudentRepository(context);
            _facultyRepo = new FacultyRepository(context);
            _departmentRepo = new DepartmentRepository(context);
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
                Role = RoleType.Admin,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty1")),
                DepartmentId = 1,
            };
            var faculty2 = new Faculty()
            {
                Name = "faculty2",
                Email = "faculty2@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Proffesors,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty2")),
                DepartmentId = 2,
            };
            var faculty3 = new Faculty()
            {
                Name = "faculty3",
                Email = "faculty3@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Associate_Proffesors,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("faculty3")),
                DepartmentId = 1,
            };
            await context.Faculties.AddRangeAsync(faculty1, faculty2, faculty3);

            var department1 = new Department { Name = "Computer Science", HeadId = 1 };
            var department2 = new Department { Name = "IT", HeadId = 1 };

            await context.Departments.AddRangeAsync(department1, department2);


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

            await context.SaveChangesAsync();
        }

        private async Task ClearDatabase()
        {
            context.Students.RemoveRange(context.Students);
            context.Departments.RemoveRange(context.Departments);
            context.Faculties.RemoveRange(context.Faculties);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Success Tests
        [Test, Order(1)]
        public async Task AddDepartmentSuccess()
        {
            
            await SeedDatabaseAsync();
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            var departmentDTO = new DepartmentDTO { Name = "New Department", HeadId = 2 };

            var result = await departmentService.AddDepartment(departmentDTO);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(departmentDTO.Name));
            Assert.That(result.HeadId, Is.EqualTo(departmentDTO.HeadId));
        }

        [Test, Order(2)]
        public async Task GetDepartmentByIdSuccess()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            var result = await departmentService.GetDepartmentById(2);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("IT"));
        }

        [Test, Order(3)]
        public async Task GetAllDepartmentsSuccess()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);
            //await SeedDatabaseAsync();

            var result = await departmentService.GetAllDepartments();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test, Order(4)]
        public async Task ChangeDepartmentHeadSuccess()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);
            var result = await departmentService.ChangeDepartmentHead(1, 3);

            Assert.IsNotNull(result);
            Assert.That(result.HeadId, Is.EqualTo(3));
        }

        [Test, Order(5)]
        public async Task DeleteDepartmentSuccess()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            var result = await departmentService.DeleteDepartment(3);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("New Department"));
        }

        #endregion

        #region Failure Tests

        [Test, Order(6)]
        public async Task AddDepartmentFailure()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            var departmentDTO = new DepartmentDTO { Name = "IT", HeadId = 1 }; 

            Assert.ThrowsAsync<DepartmentAlreadyExistException>(async () => await departmentService.AddDepartment(departmentDTO));
        }

        [Test, Order(7)]
        public async Task DeleteDepartmentFailure()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            Assert.ThrowsAsync<NoSuchDepartmentExistException>(async () => await departmentService.DeleteDepartment(99)); // Non-existent ID
        }

        [Test, Order(8)]
        public async Task GetDepartmentByIdFailure()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            Assert.ThrowsAsync<NoSuchDepartmentExistException>(async () => await departmentService.GetDepartmentById(99)); // Non-existent ID
        }

        [Test, Order(9)]
        public async Task GetAllDepartmentsFailure()
        {
            await ClearDatabase(); // Ensure no data is present
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            //var result = await departmentService.GetAllDepartments();

            //Assert.IsNotNull(result);
            Assert.ThrowsAsync<NoDepartmentsExistsException>(async () => await departmentService.GetAllDepartments());
            //Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test, Order(10)]
        public async Task ChangeDepartmentHeadFailure()
        {
            IDepartmentService departmentService = new DepartmentService(_departmentRepo, _mapper, _facultyRepo);

            Assert.ThrowsAsync<NoSuchDepartmentExistException>(async () => await departmentService.ChangeDepartmentHead(99, 2)); // Non-existent department ID
        }


        #endregion


    }
}
