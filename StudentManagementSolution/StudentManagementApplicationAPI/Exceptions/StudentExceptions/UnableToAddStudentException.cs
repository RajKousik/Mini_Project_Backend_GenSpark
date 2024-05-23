using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class UnableToAddStudentException : Exception
    {
        private string msg;
        public UnableToAddStudentException()
        {
            msg = "Unable to add a student to the Database";
        }

        public UnableToAddStudentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}