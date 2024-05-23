using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.FacultyExceptions
{
    [Serializable]
    public class UnableToDeleteFacultyException : Exception
    {
        private string msg;
        public UnableToDeleteFacultyException()
        {
            msg = "Something went wrong while deleting a Faculty Record";
        }

        public UnableToDeleteFacultyException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}