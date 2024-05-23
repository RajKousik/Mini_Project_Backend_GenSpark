using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class UnableToUpdateStudentException : Exception
    {
        private string msg;
        public UnableToUpdateStudentException()
        {
            msg = "Something went wrong while updating a student record";
        }

        public UnableToUpdateStudentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}