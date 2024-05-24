using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.DepartmentExceptions
{
    [Serializable]
    public class DepartmentAlreadyExistException : Exception
    {
        private string msg;
        public DepartmentAlreadyExistException()
        {
            msg = "Department already exists";
        }

        public DepartmentAlreadyExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}