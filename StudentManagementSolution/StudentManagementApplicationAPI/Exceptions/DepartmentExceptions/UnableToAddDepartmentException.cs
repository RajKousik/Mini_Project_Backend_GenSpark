using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.DepartmentExceptions
{
    [Serializable]
    public class UnableToAddDepartmentException : Exception
    {
        private string msg;
        public UnableToAddDepartmentException()
        {
            msg = "Something went wrong while adding a Department";
        }

        public UnableToAddDepartmentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}