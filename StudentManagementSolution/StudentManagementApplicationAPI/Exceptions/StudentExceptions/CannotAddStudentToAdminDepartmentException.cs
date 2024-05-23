using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class CannotAddStudentToAdminDepartmentException : Exception
    {
        private string msg;
        public CannotAddStudentToAdminDepartmentException()
        {
            msg = "Cannot Add a Student to Admin Department";
        }

        public CannotAddStudentToAdminDepartmentException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}