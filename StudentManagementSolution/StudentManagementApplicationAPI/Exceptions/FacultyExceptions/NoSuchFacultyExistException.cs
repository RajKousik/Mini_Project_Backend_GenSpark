using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.FacultyExceptions
{
    [Serializable]
    public class NoSuchFacultyExistException : Exception
    {
        private string msg;
        public NoSuchFacultyExistException()
        {
            msg = "No Such Faculty Found in the Database";
        }

        public NoSuchFacultyExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}