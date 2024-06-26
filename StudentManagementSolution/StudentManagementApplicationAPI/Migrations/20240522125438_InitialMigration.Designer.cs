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
    [Migration("20240522125438_InitialMigration")]
    partial class InitialMigration
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

                    b.Property<string>("ExamType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                        .IsRequired()
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
                            HashedPassword = new byte[] { 249, 176, 129, 127, 10, 247, 163, 138, 31, 109, 79, 26, 22, 148, 73, 87, 234, 255, 117, 147, 73, 56, 76, 68, 64, 223, 25, 118, 214, 95, 36, 133, 60, 203, 59, 86, 129, 43, 97, 173, 28, 4, 72, 65, 117, 100, 77, 68, 135, 236, 101, 74, 97, 253, 145, 237, 53, 21, 121, 183, 14, 134, 72, 204 },
                            Mobile = "9876523418",
                            Name = "Kousik Raj",
                            PasswordHashKey = new byte[] { 5, 124, 204, 155, 240, 244, 169, 244, 130, 245, 207, 232, 58, 119, 147, 163, 182, 190, 94, 38, 162, 162, 149, 101, 107, 234, 239, 32, 73, 101, 213, 82, 166, 60, 73, 146, 218, 48, 119, 182, 156, 45, 84, 66, 232, 69, 188, 167, 62, 229, 189, 88, 49, 126, 132, 30, 23, 106, 198, 145, 108, 146, 70, 149, 235, 36, 77, 65, 30, 136, 49, 64, 101, 185, 46, 157, 245, 72, 192, 166, 104, 189, 137, 94, 204, 109, 76, 43, 100, 211, 42, 253, 34, 200, 238, 9, 16, 107, 238, 234, 224, 215, 226, 149, 201, 156, 157, 47, 211, 8, 114, 172, 162, 55, 61, 197, 77, 83, 200, 0, 104, 253, 152, 164, 159, 2, 130, 179 },
                            Role = 0,
                            Status = 0
                        },
                        new
                        {
                            FacultyId = 102,
                            Address = "Kerala",
                            DOB = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartmentId = 12,
                            Email = "dany@gmail.com",
                            Gender = "Male",
                            HashedPassword = new byte[] { 18, 134, 1, 179, 17, 148, 56, 249, 224, 163, 0, 179, 198, 10, 150, 40, 95, 109, 122, 79, 86, 94, 64, 143, 14, 54, 127, 234, 111, 92, 25, 52, 11, 236, 102, 209, 212, 57, 68, 11, 117, 24, 99, 231, 127, 155, 124, 187, 215, 252, 138, 36, 194, 156, 24, 130, 245, 170, 155, 250, 248, 135, 26, 93 },
                            Mobile = "9187632818",
                            Name = "Dany",
                            PasswordHashKey = new byte[] { 5, 124, 204, 155, 240, 244, 169, 244, 130, 245, 207, 232, 58, 119, 147, 163, 182, 190, 94, 38, 162, 162, 149, 101, 107, 234, 239, 32, 73, 101, 213, 82, 166, 60, 73, 146, 218, 48, 119, 182, 156, 45, 84, 66, 232, 69, 188, 167, 62, 229, 189, 88, 49, 126, 132, 30, 23, 106, 198, 145, 108, 146, 70, 149, 235, 36, 77, 65, 30, 136, 49, 64, 101, 185, 46, 157, 245, 72, 192, 166, 104, 189, 137, 94, 204, 109, 76, 43, 100, 211, 42, 253, 34, 200, 238, 9, 16, 107, 238, 234, 224, 215, 226, 149, 201, 156, 157, 47, 211, 8, 114, 172, 162, 55, 61, 197, 77, 83, 200, 0, 104, 253, 152, 164, 159, 2, 130, 179 },
                            Role = 4,
                            Status = 1
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

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("AttendanceStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

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
