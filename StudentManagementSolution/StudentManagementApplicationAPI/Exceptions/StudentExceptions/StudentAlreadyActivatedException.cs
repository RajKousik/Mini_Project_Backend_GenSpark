using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.StudentExceptions
{
    [Serializable]
    public class StudentAlreadyActivatedException : Exception
    {
        private string msg;
        public StudentAlreadyActivatedException()
        {
            msg = "Student Account already activated";
        }

        public StudentAlreadyActivatedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}