using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class CannotDeleteFinishedExamException : Exception
    {
        private string msg;
        public CannotDeleteFinishedExamException()
        {
            msg = "Exam has already finished and cannot be deleted.";
        }

        public CannotDeleteFinishedExamException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}