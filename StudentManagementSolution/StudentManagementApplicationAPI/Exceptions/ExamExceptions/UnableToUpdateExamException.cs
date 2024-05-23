using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class UnableToUpdateExamException : Exception
    {
        private string msg;
        public UnableToUpdateExamException()
        {
            msg = "Something went wrong while updating a Exam Record";
        }

        public UnableToUpdateExamException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}