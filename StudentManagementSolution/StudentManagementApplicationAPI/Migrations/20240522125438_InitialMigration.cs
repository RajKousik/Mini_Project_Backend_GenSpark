using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementApplicationAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseRegistrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRegistrations", x => x.RegistrationId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    ExamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TotalMark = table.Column<int>(type: "int", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExamType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.ExamId);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DeptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HeadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DeptId);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                    table.ForeignKey(
                        name: "FK_Faculties_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DeptId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentRollNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentRollNo);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DeptId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    EvaluatedById = table.Column<int>(type: "int", nullable: false),
                    MarksScored = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    StudentGrade = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Exams_ExamId1",
                        column: x => x.ExamId1,
                        principalTable: "Exams",
                        principalColumn: "ExamId");
                    table.ForeignKey(
                        name: "FK_Grades_Faculties_EvaluatedById",
                        column: x => x.EvaluatedById,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentRollNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentAttendances",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentRollNo = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendances", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Students_StudentRollNo",
                        column: x => x.StudentRollNo,
                        principalTable: "Students",
                        principalColumn: "StudentRollNo",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "FacultyId", "Address", "DOB", "DepartmentId", "Email", "Gender", "HashedPassword", "Mobile", "Name", "PasswordHashKey", "Role", "Status" },
                values: new object[] { 101, "Chennai", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "kousik@gmail.com", "Male", new byte[] { 249, 176, 129, 127, 10, 247, 163, 138, 31, 109, 79, 26, 22, 148, 73, 87, 234, 255, 117, 147, 73, 56, 76, 68, 64, 223, 25, 118, 214, 95, 36, 133, 60, 203, 59, 86, 129, 43, 97, 173, 28, 4, 72, 65, 117, 100, 77, 68, 135, 236, 101, 74, 97, 253, 145, 237, 53, 21, 121, 183, 14, 134, 72, 204 }, "9876523418", "Kousik Raj", new byte[] { 5, 124, 204, 155, 240, 244, 169, 244, 130, 245, 207, 232, 58, 119, 147, 163, 182, 190, 94, 38, 162, 162, 149, 101, 107, 234, 239, 32, 73, 101, 213, 82, 166, 60, 73, 146, 218, 48, 119, 182, 156, 45, 84, 66, 232, 69, 188, 167, 62, 229, 189, 88, 49, 126, 132, 30, 23, 106, 198, 145, 108, 146, 70, 149, 235, 36, 77, 65, 30, 136, 49, 64, 101, 185, 46, 157, 245, 72, 192, 166, 104, 189, 137, 94, 204, 109, 76, 43, 100, 211, 42, 253, 34, 200, 238, 9, 16, 107, 238, 234, 224, 215, 226, 149, 201, 156, 157, 47, 211, 8, 114, 172, 162, 55, 61, 197, 77, 83, 200, 0, 104, 253, 152, 164, 159, 2, 130, 179 }, 0, 0 });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "FacultyId", "Address", "DOB", "DepartmentId", "Email", "Gender", "HashedPassword", "Mobile", "Name", "PasswordHashKey", "Role", "Status" },
                values: new object[] { 102, "Kerala", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "dany@gmail.com", "Male", new byte[] { 18, 134, 1, 179, 17, 148, 56, 249, 224, 163, 0, 179, 198, 10, 150, 40, 95, 109, 122, 79, 86, 94, 64, 143, 14, 54, 127, 234, 111, 92, 25, 52, 11, 236, 102, 209, 212, 57, 68, 11, 117, 24, 99, 231, 127, 155, 124, 187, 215, 252, 138, 36, 194, 156, 24, 130, 245, 170, 155, 250, 248, 135, 26, 93 }, "9187632818", "Dany", new byte[] { 5, 124, 204, 155, 240, 244, 169, 244, 130, 245, 207, 232, 58, 119, 147, 163, 182, 190, 94, 38, 162, 162, 149, 101, 107, 234, 239, 32, 73, 101, 213, 82, 166, 60, 73, 146, 218, 48, 119, 182, 156, 45, 84, 66, 232, 69, 188, 167, 62, 229, 189, 88, 49, 126, 132, 30, 23, 106, 198, 145, 108, 146, 70, 149, 235, 36, 77, 65, 30, 136, 49, 64, 101, 185, 46, 157, 245, 72, 192, 166, 104, 189, 137, 94, 204, 109, 76, 43, 100, 211, 42, 253, 34, 200, 238, 9, 16, 107, 238, 234, 224, 215, 226, 149, 201, 156, 157, 47, 211, 8, 114, 172, 162, 55, 61, 197, 77, 83, 200, 0, 104, 253, 152, 164, 159, 2, 130, 179 }, 4, 1 });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DeptId", "HeadId", "Name" },
                values: new object[] { 1, 102, "CSE" });

            

            migrationBuilder.CreateIndex(
                name: "IX_CourseRegistrations_CourseId",
                table: "CourseRegistrations",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRegistrations_StudentId",
                table: "CourseRegistrations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FacultyId",
                table: "Courses",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HeadId",
                table: "Departments",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_Email",
                table: "Faculties",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_EvaluatedById",
                table: "Grades",
                column: "EvaluatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ExamId",
                table: "Grades",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ExamId1",
                table: "Grades",
                column: "ExamId1");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_CourseId",
                table: "StudentAttendances",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentRollNo",
                table: "StudentAttendances",
                column: "StudentRollNo");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRegistrations_Courses_CourseId",
                table: "CourseRegistrations",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRegistrations_Students_StudentId",
                table: "CourseRegistrations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentRollNo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Faculties_FacultyId",
                table: "Courses",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Faculties_HeadId",
                table: "Departments",
                column: "HeadId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Faculties_HeadId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "CourseRegistrations");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "StudentAttendances");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
