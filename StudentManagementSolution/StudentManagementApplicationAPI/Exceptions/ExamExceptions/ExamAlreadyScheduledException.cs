using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class ExamAlreadyScheduledException : Exception
    {
        private string msg;
        public ExamAlreadyScheduledException()
        {
            msg = "Exam Already scheduled for this particular course";
        }

        public ExamAlreadyScheduledException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}