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
    [Migration("20240619103343_AddedApprovalStatus")]
    partial class AddedApprovalStatus
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

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

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
                            HashedPassword = new byte[] { 133, 188, 156, 144, 149, 214, 108, 127, 4, 42, 21, 123, 140, 249, 216, 112, 6, 234, 23, 133, 196, 8, 132, 254, 166, 109, 202, 224, 116, 159, 169, 183, 54, 98, 160, 209, 200, 32, 83, 247, 88, 185, 236, 181, 182, 155, 206, 13, 23, 41, 149, 112, 246, 63, 240, 187, 230, 153, 44, 30, 152, 163, 233, 173 },
                            Mobile = "9876523418",
                            Name = "Kousik Raj",
                            PasswordHashKey = new byte[] { 215, 30, 143, 227, 46, 38, 34, 89, 92, 209, 38, 37, 0, 114, 138, 234, 141, 173, 32, 148, 225, 141, 245, 96, 125, 63, 114, 23, 157, 112, 110, 245, 238, 179, 18, 1, 1, 112, 42, 110, 217, 215, 96, 199, 240, 72, 105, 87, 188, 234, 47, 220, 210, 80, 155, 98, 210, 214, 161, 226, 167, 139, 127, 69, 105, 63, 234, 154, 118, 30, 192, 139, 63, 84, 244, 114, 29, 156, 115, 19, 243, 145, 98, 241, 200, 144, 43, 72, 194, 95, 204, 126, 85, 122, 181, 154, 242, 20, 132, 38, 236, 156, 198, 19, 22, 28, 102, 241, 23, 180, 218, 166, 55, 78, 132, 33, 2, 204, 9, 197, 240, 227, 169, 95, 38, 4, 157, 230 },
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
                            HashedPassword = new byte[] { 205, 9, 10, 117, 41, 241, 27, 2, 162, 154, 160, 107, 199, 185, 185, 35, 204, 231, 218, 190, 114, 100, 238, 52, 237, 82, 91, 81, 206, 184, 186, 4, 163, 64, 189, 178, 127, 0, 58, 216, 183, 216, 165, 72, 62, 37, 72, 45, 7, 6, 217, 242, 6, 253, 33, 89, 113, 163, 58, 223, 137, 209, 243, 135 },
                            Mobile = "9187632818",
                            Name = "Dany",
                            PasswordHashKey = new byte[] { 215, 30, 143, 227, 46, 38, 34, 89, 92, 209, 38, 37, 0, 114, 138, 234, 141, 173, 32, 148, 225, 141, 245, 96, 125, 63, 114, 23, 157, 112, 110, 245, 238, 179, 18, 1, 1, 112, 42, 110, 217, 215, 96, 199, 240, 72, 105, 87, 188, 234, 47, 220, 210, 80, 155, 98, 210, 214, 161, 226, 167, 139, 127, 69, 105, 63, 234, 154, 118, 30, 192, 139, 63, 84, 244, 114, 29, 156, 115, 19, 243, 145, 98, 241, 200, 144, 43, 72, 194, 95, 204, 126, 85, 122, 181, 154, 242, 20, 132, 38, 236, 156, 198, 19, 22, 28, 102, 241, 23, 180, 218, 166, 55, 78, 132, 33, 2, 204, 9, 197, 240, 227, 169, 95, 38, 4, 157, 230 },
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
