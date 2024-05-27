using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementTest.ServiceTest.FacultyServiceTest
{
    public class FacultyServiceTest
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
                                                                .UseInMemoryDatabase("dummyFacultyServiceDB");
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
            await context.Faculties.AddRangeAsync(faculty1);

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

        private async Task AddFaculties()
        {
            var hmac = new HMACSHA512();
            var prof1 = new Faculty()
            {
                Name = "prof1",
                Email = "prof1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Proffesors,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("prof1")),
                DepartmentId = 1,
            };
            var asstprof1 = new Faculty()
            {
                Name = "asstprof1",
                Email = "asstprof1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Assistant_Proffesors,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("asstprof1")),
                DepartmentId = 2,
            };
            var asscprof1 = new Faculty()
            {
                Name = "asscprof1",
                Email = "asscprof1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Associate_Proffesors,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("asscprof1")),
                DepartmentId = 1,
            };
            var head1 = new Faculty()
            {
                Name = "head1",
                Email = "head1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Chennai",
                Mobile = "9876523418",
                Role = RoleType.Head_Of_Department,
                Status = ActivationStatus.Inactive,
                PasswordHashKey = hmac.Key,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("head1")),
                DepartmentId = 2,
            };
            await context.Faculties.AddRangeAsync(prof1, asstprof1, asscprof1, head1);
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
        public async Task UpdateFacultySuccess()
        {
            await SeedDatabaseAsync();
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var facultyDto = new FacultyDTO
            {
                Name = "Updated Faculty",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Updated Address",
                Mobile = "9876523418",
            };

            var updatedFaculty = await facultyService.UpdateFaculty(facultyDto, "faculty1@gmail.com");

            Assert.That(updatedFaculty.Name, Is.EqualTo("Updated Faculty"));
            Assert.That(updatedFaculty.Address, Is.EqualTo("Updated Address"));
        }

        [Test, Order(2)]
        public async Task GetFacultyByIdSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var faculty = await facultyService.GetFacultyById(1);

            Assert.IsNotNull(faculty);
            Assert.That(faculty.Name, Is.EqualTo("Updated Faculty"));
        }

        [Test, Order(3)]
        public async Task GetFacultyByEmailSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var faculty = await facultyService.GetFacultyByEmail("faculty1@gmail.com");

            Assert.IsNotNull(faculty);
            Assert.That(faculty.Name, Is.EqualTo("Updated Faculty"));
        }

        [Test, Order(4)]
        public async Task ChangeDepartmentSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var faculty = await facultyService.ChangeDepartment(1, 2);

            //Assert.That(faculty.DepartmentId, Is.EqualTo(2));
            Assert.IsNotNull(faculty);
        }

        [Test, Order(5)]
        public async Task GetFacultyByNameSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var faculties = await facultyService.GetFacultyByName("Updated Faculty");

            Assert.IsNotEmpty(faculties);
            Assert.That(faculties.Count(), Is.EqualTo(1));
        }

        [Test, Order(6)]
        public async Task GetAllFacultiesSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var faculties = await facultyService.GetAll();

            Assert.IsNotEmpty(faculties);
            Assert.That(faculties.Count(), Is.EqualTo(1));
        }

        [Test, Order(7)]
        public async Task DeleteFacultySuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var deletedFaculty = await facultyService.DeleteFaculty("faculty1@gmail.com");

            Assert.That(deletedFaculty.Name, Is.EqualTo("Updated Faculty"));
            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyService.GetFacultyByEmail("faculty1@gmail.com"));
        }

        [Test, Order(8)]
        public async Task GetProfessorsSuccess()
        {
            await AddFaculties();
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var professors = await facultyService.GetProfessors();

            Assert.IsNotNull(professors);
            Assert.IsNotEmpty(professors);
        }

        [Test, Order(9)]
        public async Task GetAssociateProfessorsSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var associateProfessors = await facultyService.GetAssociateProfessors();

            Assert.IsNotNull(associateProfessors);
            Assert.IsNotEmpty(associateProfessors);
        }

        [Test, Order(10)]
        public async Task GetAssistantProfessorsSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var assistantProfessors = await facultyService.GetAssistantProfessors();

            Assert.IsNotNull(assistantProfessors);
            Assert.IsNotEmpty(assistantProfessors);
        }

        [Test, Order(11)]
        public async Task GetHeadOfDepartmentSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var headOfDepartments = await facultyService.GetHeadOfDepartment();

            Assert.IsNotNull(headOfDepartments);
            Assert.IsNotEmpty(headOfDepartments);
        }

        [Test, Order(12)]
        public async Task GetFacultiesByDepartmentSuccess()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var faculties = await facultyService.GetFacultiesByDepartment(1);

            Assert.IsNotNull(faculties);
            Assert.IsNotEmpty(faculties);
        }
        #endregion

        #region Failure Tests

        [Test, Order(13)]
        public async Task UpdateFacultyFailure()
        {
            await ClearDatabase();
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            var facultyDto = new FacultyDTO
            {
                Name = "NonExisting Faculty",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Updated Address",
                Mobile = "9876523418",
            };

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyService.UpdateFaculty(facultyDto, "nonexisting@gmail.com"));
        }

        [Test, Order(14)]
        public async Task GetFacultyByIdFailure()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyService.GetFacultyById(99));
        }

        [Test, Order(15)]
        public async Task GetFacultyByEmailFailure()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyService.GetFacultyByEmail("nonexisting@gmail.com"));
        }

        [Test, Order(16)]
        public async Task ChangeDepartmentFailure()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyService.ChangeDepartment(99, 2));
        }

        [Test, Order(17)]
        public async Task GetFacultyByNameFailure()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            //var faculties = await facultyService.GetFacultyByName("NonExisting Faculty");
            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetFacultyByName("NonExisting Faculty"));
            //Assert.IsEmpty(faculties);
        }

        [Test, Order(18)]
        public async Task GetAllFacultiesFailure()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);
            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetAll());
        }

        [Test, Order(19)]
        public async Task DeleteFacultyFailure()
        {
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoSuchFacultyExistException>(async () => await facultyService.DeleteFaculty("nonexisting@gmail.com"));
        }

        [Test, Order(20)]
        public async Task GetProfessorsFailure()
        {
            await ClearDatabase(); // Ensure no data is present
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetProfessors());
        }

        [Test, Order(21)]
        public async Task GetAssociateProfessorsFailure()
        {
            await ClearDatabase(); // Ensure no data is present
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetAssociateProfessors());
        }

        [Test, Order(22)]
        public async Task GetAssistantProfessorsFailure()
        {
            await ClearDatabase(); // Ensure no data is present
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetAssistantProfessors());
        }

        [Test, Order(23)]
        public async Task GetHeadOfDepartmentFailure()
        {
            await ClearDatabase(); // Ensure no data is present
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetHeadOfDepartment());
        }

        [Test, Order(24)]
        public async Task GetFacultiesByDepartmentFailure()
        {
            await ClearDatabase(); // Ensure no data is present
            IFacultyService facultyService = new FacultyService(_facultyRepo, _mapper, _departmentRepo);

            Assert.ThrowsAsync<NoFacultiesExistsException>(async () => await facultyService.GetFacultiesByDepartment(99));
        }

        #endregion

    }
}
