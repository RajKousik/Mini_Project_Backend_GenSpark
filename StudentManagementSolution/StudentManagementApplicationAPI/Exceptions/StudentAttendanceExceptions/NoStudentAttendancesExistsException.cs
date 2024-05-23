using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class NoStudentAttendancesExistsException : Exception
    {
        private string msg;
        public NoStudentAttendancesExistsException()
        {
            msg = "No Student Attendance Records Found in the database";
        }

        public NoStudentAttendancesExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}