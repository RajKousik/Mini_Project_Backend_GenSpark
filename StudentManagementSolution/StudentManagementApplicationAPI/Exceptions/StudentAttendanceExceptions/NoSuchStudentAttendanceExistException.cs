using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class NoSuchStudentAttendanceExistException : Exception
    {
        private string msg;
        public NoSuchStudentAttendanceExistException()
        {
            msg = "No Such Student Attendance Record Found in the Database";
        }

        public NoSuchStudentAttendanceExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}