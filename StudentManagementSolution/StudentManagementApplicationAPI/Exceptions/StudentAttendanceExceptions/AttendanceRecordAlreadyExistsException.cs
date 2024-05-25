using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class AttendanceRecordAlreadyExistsException : Exception
    {
        private string msg;
        public AttendanceRecordAlreadyExistsException()
        {
            msg = "Attendance record for student with roll number {attendanceDTO.StudentRollNo}, course with ID {attendanceDTO.CourseId}, and date {attendanceDTO.Date} already exists.";
        }

        public AttendanceRecordAlreadyExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}