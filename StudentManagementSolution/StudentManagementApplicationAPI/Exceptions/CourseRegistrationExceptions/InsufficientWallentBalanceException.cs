using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class InsufficientWallentBalanceException : Exception
    {
        private string msg;
        public InsufficientWallentBalanceException()
        {
            msg = "Not enough balance in the student wallet";
        }

        public InsufficientWallentBalanceException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}