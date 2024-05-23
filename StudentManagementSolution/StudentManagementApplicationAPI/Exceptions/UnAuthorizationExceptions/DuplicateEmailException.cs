using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions
{
    [Serializable]
    public class DuplicateEmailException : Exception
    {
        private string msg;
        public DuplicateEmailException()
        {
            msg = "Already a user with same email address exist!";
        }

        public DuplicateEmailException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}