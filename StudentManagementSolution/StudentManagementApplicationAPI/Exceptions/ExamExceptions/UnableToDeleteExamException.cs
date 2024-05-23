using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class UnableToDeleteExamException : Exception
    {
        private string msg;
        public UnableToDeleteExamException()
        {
            msg = "Something went wrong while deleting a Exam Record";
        }

        public UnableToDeleteExamException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}