using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.DepartmentExceptions
{
    [Serializable]
    public class NoSuchDepartmentExistException : Exception
    {
        private string msg;
        public NoSuchDepartmentExistException()
        {
            msg = "No Such Course Department Found in the Database";
        }

        public NoSuchDepartmentExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}