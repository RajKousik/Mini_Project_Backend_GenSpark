using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class InvalidTotalMarkException : Exception
    {
        private string msg;
        public InvalidTotalMarkException()
        {
            msg = "TotalMark must be between 1 and 100.";
        }

        public InvalidTotalMarkException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}