using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.FacultyExceptions
{
    [Serializable]
    public class UnableToUpdateFacultyException : Exception
    {
        private string msg;
        public UnableToUpdateFacultyException()
        {
            msg = "Something went wrong while updating a Faculty Record";
        }

        public UnableToUpdateFacultyException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}