using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementApplicationAPI.Contexts
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hmac = new HMACSHA512();



            modelBuilder.Entity<Faculty>().HasData(
                new Faculty()
                {
                    FacultyId = 101,
                    Name = "Kousik Raj",
                    Email = "kousik@gmail.com",
                    DOB = new DateTime(2000, 01, 01),
                    Gender = "Male",
                    Address = "Chennai",
                    Mobile = "9876523418",
                    Role = RoleType.Admin,
                    Status = ActivationStatus.Active,
                    PasswordHashKey = hmac.Key,
                    HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("Admin123")),
                    DepartmentId = 1,
                },
                new Faculty()
                {
                    FacultyId = 102,
                    Name = "Dany",
                    Email = "dany@gmail.com",
                    DOB = new DateTime(1990, 01, 01),
                    Gender = "Male",
                    Address = "Kerala",
                    Mobile = "9187632818",
                    Role = RoleType.Head_Of_Department,
                    Status = ActivationStatus.Inactive,
                    PasswordHashKey = hmac.Key,
                    HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("Dany123")),
                    DepartmentId = 1
                }
            );

            modelBuilder.Entity<Department>().HasData(
                new Department()
                {
                    DeptId = 1,
                    Name = "CSE",
                    HeadId = 102,
                }
            );

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Head)
                .WithMany()
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                  .HasMany(d => d.Students)
                  .WithOne(s => s.Department)
                  .HasForeignKey(s => s.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Faculties)
                .WithOne(f => f.Department)
                .HasForeignKey(f => f.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.CourseRegistrations)
                .WithOne(cr => cr.Course)
                .HasForeignKey(cr => cr.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Exams)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Grades)
                .WithOne(g => g.Exam)
                .HasForeignKey(g => g.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Faculty>()
                .HasOne(f => f.Department)
                .WithMany(d => d.Faculties)
                .HasForeignKey(f => f.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Exam)
                .WithMany()
                .HasForeignKey(g => g.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.EvaluatedBy)
                .WithMany()
                .HasForeignKey(g => g.EvaluatedById)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
