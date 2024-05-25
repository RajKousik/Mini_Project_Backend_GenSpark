using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentAttendanceDTOs
{
    public class AttendanceDTO
    {
        public int StudentRollNo { get; set; }
        public int CourseId { get; set; }
        public DateOnly Date { get; set; }
        public string AttendanceStatus { get; set; }
    }
}
