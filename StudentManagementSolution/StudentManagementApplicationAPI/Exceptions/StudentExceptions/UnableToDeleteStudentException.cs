using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class UnableToDeleteStudentException : Exception
    {
        private string msg;
        public UnableToDeleteStudentException()
        {
            msg = "Something went wrong while deleting a student record";
        }

        public UnableToDeleteStudentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}