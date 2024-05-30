using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementApplicationAPI.Migrations
{
    public partial class AddedFeesStrcuture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EWallet",
                table: "Students",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Faculties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "CourseFees",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            //migrationBuilder.UpdateData(
            //    table: "Faculties",
            //    keyColumn: "FacultyId",
            //    keyValue: 101,
            //    columns: new[] { "HashedPassword", "PasswordHashKey" },
            //    values: new object[] { new byte[] { 2, 42, 52, 211, 111, 238, 3, 77, 217, 33, 89, 164, 62, 62, 63, 174, 106, 162, 70, 234, 118, 90, 153, 113, 177, 130, 111, 95, 82, 198, 249, 62, 1, 191, 172, 194, 34, 221, 99, 167, 188, 229, 255, 71, 204, 196, 70, 129, 138, 120, 72, 196, 149, 251, 246, 90, 249, 81, 213, 76, 145, 140, 0, 230 }, new byte[] { 210, 251, 86, 136, 66, 179, 18, 103, 223, 188, 233, 98, 96, 65, 60, 168, 219, 122, 160, 58, 200, 147, 183, 227, 8, 166, 231, 157, 52, 17, 237, 133, 30, 255, 188, 249, 127, 58, 22, 54, 24, 57, 232, 18, 170, 29, 245, 12, 126, 145, 32, 204, 10, 117, 53, 36, 10, 28, 177, 89, 175, 194, 249, 15, 89, 10, 78, 111, 83, 161, 62, 177, 156, 57, 63, 92, 172, 68, 65, 227, 73, 87, 171, 45, 114, 246, 197, 174, 213, 76, 184, 235, 114, 56, 16, 249, 214, 233, 98, 24, 43, 176, 211, 53, 9, 117, 205, 184, 136, 21, 166, 28, 55, 101, 33, 78, 220, 116, 229, 149, 21, 192, 138, 253, 6, 205, 181, 55 } });

            //migrationBuilder.UpdateData(
            //    table: "Faculties",
            //    keyColumn: "FacultyId",
            //    keyValue: 102,
            //    columns: new[] { "HashedPassword", "PasswordHashKey" },
            //    values: new object[] { new byte[] { 135, 252, 74, 112, 20, 143, 11, 140, 3, 146, 130, 27, 252, 181, 188, 104, 121, 187, 24, 47, 95, 8, 230, 145, 193, 54, 120, 71, 113, 113, 6, 238, 27, 125, 43, 141, 199, 34, 28, 114, 233, 141, 153, 190, 64, 112, 90, 227, 147, 6, 119, 52, 203, 113, 248, 78, 193, 200, 8, 62, 156, 246, 69, 217 }, new byte[] { 210, 251, 86, 136, 66, 179, 18, 103, 223, 188, 233, 98, 96, 65, 60, 168, 219, 122, 160, 58, 200, 147, 183, 227, 8, 166, 231, 157, 52, 17, 237, 133, 30, 255, 188, 249, 127, 58, 22, 54, 24, 57, 232, 18, 170, 29, 245, 12, 126, 145, 32, 204, 10, 117, 53, 36, 10, 28, 177, 89, 175, 194, 249, 15, 89, 10, 78, 111, 83, 161, 62, 177, 156, 57, 63, 92, 172, 68, 65, 227, 73, 87, 171, 45, 114, 246, 197, 174, 213, 76, 184, 235, 114, 56, 16, 249, 214, 233, 98, 24, 43, 176, 211, 53, 9, 117, 205, 184, 136, 21, 166, 28, 55, 101, 33, 78, 220, 116, 229, 149, 21, 192, 138, 253, 6, 205, 181, 55 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EWallet",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CourseFees",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Faculties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.UpdateData(
            //    table: "Faculties",
            //    keyColumn: "FacultyId",
            //    keyValue: 101,
            //    columns: new[] { "HashedPassword", "PasswordHashKey" },
            //    values: new object[] { new byte[] { 117, 145, 28, 122, 212, 60, 208, 187, 38, 147, 198, 194, 108, 26, 249, 132, 243, 213, 100, 241, 131, 2, 73, 23, 142, 13, 42, 205, 217, 236, 149, 245, 137, 53, 144, 105, 125, 222, 128, 172, 45, 33, 226, 114, 243, 105, 115, 21, 69, 41, 16, 204, 74, 102, 197, 86, 126, 249, 109, 199, 114, 36, 231, 0 }, new byte[] { 183, 232, 96, 153, 199, 132, 213, 99, 40, 109, 132, 41, 120, 189, 210, 81, 175, 203, 242, 91, 112, 66, 149, 204, 118, 232, 236, 89, 241, 30, 244, 19, 152, 200, 61, 250, 171, 192, 148, 249, 14, 25, 170, 112, 5, 38, 158, 253, 215, 31, 72, 97, 129, 37, 144, 197, 137, 229, 39, 104, 228, 198, 116, 71, 211, 21, 49, 240, 213, 229, 29, 244, 119, 52, 204, 41, 83, 247, 168, 56, 95, 244, 191, 20, 229, 57, 202, 72, 164, 116, 204, 197, 250, 51, 9, 246, 147, 208, 205, 34, 91, 118, 151, 243, 185, 83, 24, 146, 131, 208, 4, 51, 173, 166, 223, 212, 23, 115, 120, 180, 125, 173, 253, 193, 83, 44, 76, 87 } });

            //migrationBuilder.UpdateData(
            //    table: "Faculties",
            //    keyColumn: "FacultyId",
            //    keyValue: 102,
            //    columns: new[] { "HashedPassword", "PasswordHashKey" },
            //    values: new object[] { new byte[] { 193, 158, 23, 2, 245, 101, 212, 198, 3, 19, 17, 214, 36, 3, 172, 70, 130, 240, 244, 15, 56, 228, 48, 179, 179, 155, 133, 241, 247, 131, 162, 134, 130, 221, 77, 109, 215, 48, 12, 112, 117, 189, 27, 167, 95, 137, 235, 102, 221, 203, 167, 228, 104, 30, 126, 111, 81, 143, 221, 66, 81, 90, 129, 39 }, new byte[] { 183, 232, 96, 153, 199, 132, 213, 99, 40, 109, 132, 41, 120, 189, 210, 81, 175, 203, 242, 91, 112, 66, 149, 204, 118, 232, 236, 89, 241, 30, 244, 19, 152, 200, 61, 250, 171, 192, 148, 249, 14, 25, 170, 112, 5, 38, 158, 253, 215, 31, 72, 97, 129, 37, 144, 197, 137, 229, 39, 104, 228, 198, 116, 71, 211, 21, 49, 240, 213, 229, 29, 244, 119, 52, 204, 41, 83, 247, 168, 56, 95, 244, 191, 20, 229, 57, 202, 72, 164, 116, 204, 197, 250, 51, 9, 246, 147, 208, 205, 34, 91, 118, 151, 243, 185, 83, 24, 146, 131, 208, 4, 51, 173, 166, 223, 212, 23, 115, 120, 180, 125, 173, 253, 193, 83, 44, 76, 87 } });
        }
    }
}
