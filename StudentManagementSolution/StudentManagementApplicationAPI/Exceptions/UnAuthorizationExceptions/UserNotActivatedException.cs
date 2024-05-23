using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions
{
    [Serializable]
    public class UserNotActivatedException : Exception
    {
        private string msg;
        public UserNotActivatedException()
        {
            msg = "Something Went Wrong! User Not Activated!";
        }

        public UserNotActivatedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}