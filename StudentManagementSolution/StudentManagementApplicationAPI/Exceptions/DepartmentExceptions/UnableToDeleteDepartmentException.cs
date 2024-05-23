using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.DepartmentExceptions
{
    [Serializable]
    public class UnableToDeleteDepartmentException : Exception
    {
        private string msg;
        public UnableToDeleteDepartmentException()
        {
            msg = "Something went wrong while deleting a Department";
        }

        public UnableToDeleteDepartmentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}