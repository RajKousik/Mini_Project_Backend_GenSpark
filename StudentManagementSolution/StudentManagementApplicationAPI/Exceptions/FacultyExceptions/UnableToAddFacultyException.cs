using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.FacultyExceptions
{
    [Serializable]
    public class UnableToAddFacultyException : Exception
    {
        private string msg;
        public UnableToAddFacultyException()
        {
            msg = "Something went wrong while adding a Faculty Record";
        }

        public UnableToAddFacultyException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}