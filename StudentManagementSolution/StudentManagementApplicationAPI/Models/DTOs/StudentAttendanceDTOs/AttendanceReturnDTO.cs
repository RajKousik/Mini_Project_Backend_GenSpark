using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentAttendanceDTOs
{
    public class AttendanceReturnDTO
    {
        public int Id { get; set; }
        public int StudentRollNo { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
        public string AttendanceStatus { get; set; }
    }
}
