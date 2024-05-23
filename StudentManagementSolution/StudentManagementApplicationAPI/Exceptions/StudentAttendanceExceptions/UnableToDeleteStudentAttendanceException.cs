using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class UnableToDeleteStudentAttendanceException : Exception
    {
        private string msg;
        public UnableToDeleteStudentAttendanceException()
        {
            msg = "Something went wrong while deleting a Student Attendance";
        }

        public UnableToDeleteStudentAttendanceException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}