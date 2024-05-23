using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions
{
    [Serializable]
    public class UnauthorizedUserException : Exception
    {
        private string msg;
        public UnauthorizedUserException()
        {
            msg = "User Not Authorized!!!";
        }

        public UnauthorizedUserException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}