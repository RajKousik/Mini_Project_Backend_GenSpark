using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
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

namespace StudentManagementTest.RepositoryTest.StudentRepositoryTest
{
    public class StudentRepositoryTest
    {
        StudentManagementContext context;
        IRepository<int, Student> studentRepo;
        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                                .UseInMemoryDatabase("dummyStudentRepositoryDB");
            context = new StudentManagementContext(optionsBuilder.Options);
            studentRepo = new StudentRepository(context);
        }

        [Test, Order(1)]
        public async Task AddStudentSuccess()
        {
            var department1 = new Department { Name = "Computer Science", HeadId = 1 };

            await context.Departments.AddRangeAsync(department1);
            var hmac = new HMACSHA512();
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
            var addedStudent = await studentRepo.Add(student1);
            Assert.That(addedStudent.Email, Is.EqualTo(student1.Email));
        }

        [Test, Order(2)]
        public async Task GetAllStuentsSuccess()
        {
            var students = await studentRepo.GetAll();
            Assert.That(students.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetStudentByIdSuccess()
        {
            var student = await studentRepo.GetById(1);
            Assert.That(student.Email, Is.EqualTo("student1@gmail.com"));
        }

        [Test, Order(4)]
        public async Task UpdateStudentSuccess()
        {
            var student = await studentRepo.GetById(1);
            student.Email = "updated@gmail.com";
            var updatedStudent = await studentRepo.Update(student);
            Assert.That(updatedStudent.Email, Is.EqualTo("updated@gmail.com"));
        }

        [Test, Order(5)]
        public async Task DeleteStudentSuccess()
        {
            var student = await studentRepo.Delete(1);
            
            Assert.That(student.Email, Is.EqualTo("updated@gmail.com"));
            Assert.IsEmpty(await studentRepo.GetAll());
        }


        [Test, Order(6)]
        public void AddStudentFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await studentRepo.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllStudentsFailure()
        {
            var students = await studentRepo.GetAll();
            Assert.IsEmpty(students);
        }

        [Test, Order(8)]
        public void GetStuentByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentRepo.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateStudentFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await studentRepo.Update(null));
        }

        [Test, Order(10)]
        public void DeleteStudentFailure()
        {
            Assert.ThrowsAsync<NoSuchStudentExistException>(async () => await studentRepo.Delete(1));
        }
    }
}
