using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class NoSuchStudentExistException : Exception
    {
        private string msg;
        public NoSuchStudentExistException()
        {
            msg = "No Such Student Found in the Database";
        }

        public NoSuchStudentExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;

    }
}