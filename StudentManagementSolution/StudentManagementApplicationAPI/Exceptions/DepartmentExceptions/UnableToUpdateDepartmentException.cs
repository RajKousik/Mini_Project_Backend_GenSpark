using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.DepartmentExceptions
{
    [Serializable]
    public class UnableToUpdateDepartmentException : Exception
    {
        private string msg;
        public UnableToUpdateDepartmentException()
        {
            msg = "Something went wrong while updating a Department";
        }

        public UnableToUpdateDepartmentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}