using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services.Student_Service;
using System.Security.Cryptography;
using System.Text;


namespace StudentManagementTest.ServiceTest.StudentServiceTest
{
    public class StudentServiceTest
    {
        #region Fields
        StudentManagementContext context;
        IRepository<int, Student> _studentRepo;
        IRepository<int, Department> _departmentRepo;
        IMapper _mapper;
        MapperConfiguration _config;
        Mock<ILogger<StudentService>> mockLoggerConfig;
        #endregion

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyStudentServiceDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            _studentRepo = new StudentRepository(context);
            _departmentRepo = new DepartmentRepository(context);
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "StudentManagementApplicationAPI"
            }));
            _mapper = _config.CreateMapper();

            mockLoggerConfig = new Mock<ILogger<StudentService>>();
        }

        private async Task SeedDatabaseAsync()
        {
            var hmac = new HMACSHA512();
            var faculty1 = new Faculty
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

            await context.Departments.AddRangeAsync(department1);


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


        [Test, Order(1)]
        public async Task UpdateStudentSuccess()
        {
            await SeedDatabaseAsync();
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            var studentDto = new StudentDTO
            {
                Name = "Updated Student",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Updated Address",
                Mobile = "9876523418",
                DepartmentId = 1
            };

            var updatedStudent = await studentService.UpdateStudent(studentDto, "student1@gmail.com");

            Assert.That(updatedStudent.Name, Is.EqualTo("Updated Student"));
            Assert.That(updatedStudent.Address, Is.EqualTo("Updated Address"));

        }


        [Test, Order(2)]
        public async Task GetStudentByIdSuccess()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);


            var student = await studentService.GetStudentById(1);

            Assert.IsNotNull(student);
            Assert.That(student.Name, Is.EqualTo("Updated Student"));

        }


        [Test, Order(3)]
        public async Task GetStudentByEmailSuccess()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            var student = await studentService.GetStudentByEmail("student1@gmail.com");

            Assert.IsNotNull(student);
            Assert.That(student.Name, Is.EqualTo("Updated Student"));

        }


        [Test, Order(4)]
        public async Task GetStudentByNameSuccess()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            var students = await studentService.GetStudentByName("Updated Student");

            Assert.IsNotEmpty(students);
            Assert.That(students.Count(), Is.EqualTo(1));

        }


        [Test, Order(5)]
        public async Task GetAllStudentsSuccessWithData()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);
            var students = await studentService.GetAllStudents();

            Assert.IsNotEmpty(students);
            Assert.That(students.Count(), Is.EqualTo(1));
        }


        [Test, Order(6)]
        public async Task GetStudentsByDepartmentSuccess()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);
            var students = await studentService.GetStudentsByDepartment(1);

            Assert.IsNotEmpty(students);
            Assert.That(students.Count(), Is.EqualTo(1));
        }


        [Test, Order(7)]
        public async Task DeleteStudentSuccess()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);
            var deletedStudent = await studentService.DeleteStudent("student1@gmail.com");

            Assert.That(deletedStudent.Name, Is.EqualTo("Updated Student"));
            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentService.GetStudentByEmail("student1@gmail.com"));
        }

        [Test, Order(8)]
        public void UpdateStudentFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            var studentDto = new StudentDTO
            {
                Name = "Updated Student",
                DOB = new DateTime(2000, 01, 01),
                Gender = "Male",
                Address = "Updated Address",
                Mobile = "9876523418",
                DepartmentId = 1
            };

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentService.UpdateStudent(studentDto, "nonexistent@gmail.com"));
        }

        [Test, Order(9)]
        public void GetStudentByIdFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentService.GetStudentById(999));
        }

        [Test, Order(10)]
        public void GetStudentByEmailFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentService.GetStudentByEmail("nonexistent@gmail.com"));
        }

        [Test, Order(11)]
        public void GetStudentByNameFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            //var students = await studentService.GetStudentByName("Nonexistent Student");

            Assert.ThrowsAsync<NoStudentsExistsException>(async () => await studentService.GetStudentByName("Nonexistent Student"));
        }

        [Test, Order(12)]
        public void GetAllStudentsFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoStudentsExistsException>(async () => await studentService.GetAllStudents());
        }

        [Test, Order(13)]
        public void GetStudentsByDepartmentFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            //var students = await studentService.GetStudentsByDepartment(999); // Assuming 999 is a nonexistent department ID
            Assert.ThrowsAsync<NoSuchDepartmentExistException>(async () => await studentService.GetStudentsByDepartment(999));

            Assert.ThrowsAsync<NoStudentsExistsException>(async () => await studentService.GetStudentsByDepartment(1));
            //Assert.IsEmpty(students);
        }

        [Test, Order(14)]
        public void DeleteStudentFailure()
        {
            IStudentService studentService = new StudentService(_studentRepo, _mapper, _departmentRepo, mockLoggerConfig.Object);

            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentService.DeleteStudent("nonexistent@gmail.com"));
        }


    }
}

