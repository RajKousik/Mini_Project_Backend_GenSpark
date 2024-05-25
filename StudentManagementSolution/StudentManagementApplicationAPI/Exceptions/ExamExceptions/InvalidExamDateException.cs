using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class InvalidExamDateException : Exception
    {
        private string msg;
        public InvalidExamDateException()
        {
            msg = "ExamDate must be in the future";
        }

        public InvalidExamDateException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}