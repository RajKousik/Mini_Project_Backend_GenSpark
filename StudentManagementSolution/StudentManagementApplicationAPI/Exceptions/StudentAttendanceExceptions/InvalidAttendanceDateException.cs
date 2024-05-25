using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class InvalidAttendanceDateException : Exception
    {
        private string msg;
        public InvalidAttendanceDateException()
        {
            msg = "Attendance date cannot be in the future.";
        }

        public InvalidAttendanceDateException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}