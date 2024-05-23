using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class UnableToUpdateStudentAttendanceException : Exception
    {
        private string msg;
        public UnableToUpdateStudentAttendanceException()
        {
            msg = "Something went wrong while updating a Student Attendance";
        }

        public UnableToUpdateStudentAttendanceException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}