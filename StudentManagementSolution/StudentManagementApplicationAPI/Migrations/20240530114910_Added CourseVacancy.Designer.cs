﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagementApplicationAPI.Contexts;

#nullable disable

namespace StudentManagementApplicationAPI.Migrations
{
    [DbContext(typeof(StudentManagementContext))]
    [Migration("20240530114910_Added CourseVacancy")]
    partial class AddedCourseVacancy
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"), 1L, 1);

                    b.Property<double>("CourseFees")
                        .HasColumnType("float");

                    b.Property<int>("CourseVacancy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CourseId");

                    b.HasIndex("FacultyId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.CourseRegistration", b =>
                {
                    b.Property<int>("RegistrationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistrationId"), 1L, 1);

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("RegistrationId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("CourseRegistrations");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Department", b =>
                {
                    b.Property<int>("DeptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeptId"), 1L, 1);

                    b.Property<int?>("HeadId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DeptId");

                    b.HasIndex("HeadId");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DeptId = 1,
                            HeadId = 102,
                            Name = "CSE"
                        });
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Exam", b =>
                {
                    b.Property<int>("ExamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExamId"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExamDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExamType")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalMark")
                        .HasColumnType("int");

                    b.HasKey("ExamId");

                    b.HasIndex("CourseId")
                        .IsUnique();

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Faculty", b =>
                {
                    b.Property<int>("FacultyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacultyId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("FacultyId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Faculties");

                    b.HasData(
                        new
                        {
                            FacultyId = 101,
                            Address = "Chennai",
                            DOB = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentId = 1,
                            Email = "kousik@gmail.com",
                            Gender = "Male",
                            HashedPassword = new byte[] { 176, 68, 100, 64, 243, 109, 215, 221, 44, 79, 35, 28, 204, 255, 49, 17, 106, 207, 142, 156, 241, 17, 254, 37, 194, 19, 100, 17, 130, 29, 104, 48, 43, 128, 238, 187, 13, 227, 104, 11, 113, 239, 218, 42, 32, 149, 163, 29, 124, 30, 9, 53, 11, 41, 211, 234, 7, 51, 215, 200, 127, 141, 96, 30 },
                            Mobile = "9876523418",
                            Name = "Kousik Raj",
                            PasswordHashKey = new byte[] { 32, 16, 57, 218, 31, 42, 153, 211, 213, 201, 7, 35, 93, 218, 74, 171, 111, 90, 203, 2, 70, 49, 61, 77, 61, 16, 110, 241, 148, 1, 94, 126, 173, 237, 178, 235, 96, 112, 13, 85, 151, 121, 64, 107, 181, 197, 215, 198, 50, 37, 7, 31, 26, 69, 197, 216, 241, 217, 23, 60, 132, 219, 93, 157, 231, 118, 108, 132, 48, 153, 37, 216, 188, 55, 225, 156, 181, 148, 112, 17, 210, 210, 33, 180, 218, 194, 237, 226, 68, 34, 42, 62, 102, 183, 179, 81, 207, 21, 59, 73, 220, 43, 210, 223, 157, 55, 169, 249, 234, 222, 128, 210, 150, 208, 175, 223, 136, 154, 104, 142, 230, 13, 103, 72, 48, 233, 62, 164 },
                            Role = 0,
                            Status = 1
                        },
                        new
                        {
                            FacultyId = 102,
                            Address = "Kerala",
                            DOB = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentId = 1,
                            Email = "dany@gmail.com",
                            Gender = "Male",
                            HashedPassword = new byte[] { 175, 34, 75, 165, 60, 227, 100, 246, 176, 12, 178, 50, 206, 205, 174, 229, 22, 209, 210, 197, 170, 60, 18, 90, 149, 6, 214, 5, 107, 196, 90, 115, 177, 137, 161, 183, 178, 210, 151, 243, 118, 232, 20, 1, 201, 67, 53, 24, 162, 233, 223, 88, 228, 231, 239, 184, 17, 84, 0, 173, 107, 46, 199, 118 },
                            Mobile = "9187632818",
                            Name = "Dany",
                            PasswordHashKey = new byte[] { 32, 16, 57, 218, 31, 42, 153, 211, 213, 201, 7, 35, 93, 218, 74, 171, 111, 90, 203, 2, 70, 49, 61, 77, 61, 16, 110, 241, 148, 1, 94, 126, 173, 237, 178, 235, 96, 112, 13, 85, 151, 121, 64, 107, 181, 197, 215, 198, 50, 37, 7, 31, 26, 69, 197, 216, 241, 217, 23, 60, 132, 219, 93, 157, 231, 118, 108, 132, 48, 153, 37, 216, 188, 55, 225, 156, 181, 148, 112, 17, 210, 210, 33, 180, 218, 194, 237, 226, 68, 34, 42, 62, 102, 183, 179, 81, 207, 21, 59, 73, 220, 43, 210, 223, 157, 55, 169, 249, 234, 222, 128, 210, 150, 208, 175, 223, 136, 154, 104, 142, 230, 13, 103, 72, 48, 233, 62, 164 },
                            Role = 4,
                            Status = -1
                        });
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EvaluatedById")
                        .HasColumnType("int");

                    b.Property<int>("ExamId")
                        .HasColumnType("int");

                    b.Property<int?>("ExamId1")
                        .HasColumnType("int");

                    b.Property<int>("MarksScored")
                        .HasColumnType("int");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.Property<int>("StudentGrade")
                        .HasMaxLength(1)
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EvaluatedById");

                    b.HasIndex("ExamId");

                    b.HasIndex("ExamId1");

                    b.HasIndex("StudentId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Student", b =>
                {
                    b.Property<int>("StudentRollNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentRollNo"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<double>("EWallet")
                        .HasColumnType("float");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("StudentRollNo");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.StudentAttendance", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("AttendanceStatus")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentRollNo")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentRollNo");

                    b.ToTable("StudentAttendances");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Course", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.CourseRegistration", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Course", "Course")
                        .WithMany("CourseRegistrations")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Department", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Faculty", "Head")
                        .WithMany()
                        .HasForeignKey("HeadId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Head");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Exam", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Course", "Course")
                        .WithMany("Exams")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Faculty", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Department", "Department")
                        .WithMany("Faculties")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Department");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Grade", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Faculty", "EvaluatedBy")
                        .WithMany()
                        .HasForeignKey("EvaluatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Exam", "Exam")
                        .WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Exam", null)
                        .WithMany("Grades")
                        .HasForeignKey("ExamId1");

                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("EvaluatedBy");

                    b.Navigation("Exam");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Student", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.StudentAttendance", b =>
                {
                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagementApplicationAPI.Models.Db_Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentRollNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Course", b =>
                {
                    b.Navigation("CourseRegistrations");

                    b.Navigation("Exams");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Department", b =>
                {
                    b.Navigation("Faculties");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("StudentManagementApplicationAPI.Models.Db_Models.Exam", b =>
                {
                    b.Navigation("Grades");
                });
#pragma warning restore 612, 618
        }
    }
}
