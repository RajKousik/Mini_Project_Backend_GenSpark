using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class UnableToAddStudentAttendanceException : Exception
    {
        private string msg;
        public UnableToAddStudentAttendanceException()
        {
            msg = "Something went wrong while adding a Student Attendance";
        }

        public UnableToAddStudentAttendanceException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}