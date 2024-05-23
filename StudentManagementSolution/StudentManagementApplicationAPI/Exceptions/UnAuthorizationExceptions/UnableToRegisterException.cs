using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions
{
    [Serializable]
    public class UnableToRegisterException : Exception
    {
        private string msg;
        public UnableToRegisterException()
        {
            msg = "Something Went Wrong! Unable to resgiter at this moment!";
        }

        public UnableToRegisterException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}