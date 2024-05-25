using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions
{
    [Serializable]
    public class InvalidAttendanceUpdateException : Exception
    {
        private string msg;
        public InvalidAttendanceUpdateException()
        {
            msg = "Cannot update the same status again";
        }

        public InvalidAttendanceUpdateException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}