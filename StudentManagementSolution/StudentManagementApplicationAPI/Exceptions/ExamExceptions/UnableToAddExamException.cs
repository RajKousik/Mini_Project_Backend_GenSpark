using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class UnableToAddExamException : Exception
    {
        private string msg;
        public UnableToAddExamException()
        {
            msg = "Something went wrong while adding a Exam Record";
        }

        public UnableToAddExamException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}