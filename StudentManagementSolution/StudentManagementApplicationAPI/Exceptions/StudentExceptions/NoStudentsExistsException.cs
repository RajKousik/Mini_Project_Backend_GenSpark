using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class NoStudentsExistsException : Exception
    {
        private string msg;
        public NoStudentsExistsException()
        {
            msg = "No students Found in the database";
        }

        public NoStudentsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;

        
    }
}