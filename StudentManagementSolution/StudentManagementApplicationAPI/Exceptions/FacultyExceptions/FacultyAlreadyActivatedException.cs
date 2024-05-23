using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.FacultyExceptions
{
    [Serializable]
    public class FacultyAlreadyActivatedException : Exception
    {
        private string msg;
        public FacultyAlreadyActivatedException()
        {
            msg = "Faculty Account already activated";
        }

        public FacultyAlreadyActivatedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}