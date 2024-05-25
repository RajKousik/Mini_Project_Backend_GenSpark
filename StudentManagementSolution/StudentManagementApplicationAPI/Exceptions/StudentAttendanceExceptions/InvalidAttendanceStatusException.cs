using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class InvalidAttendanceStatusException : Exception
    {
        private string msg;
        public InvalidAttendanceStatusException()
        {
            msg = "Attendance Status should be either 'Present', 'Absent' or 'Od'";
        }

        public InvalidAttendanceStatusException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}