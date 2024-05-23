using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.DepartmentExceptions
{
    [Serializable]
    public class NoDepartmentsExistsException : Exception
    {
        private string msg;
        public NoDepartmentsExistsException()
        {
            msg = "No Departments Found in the database";
        }

        public NoDepartmentsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}