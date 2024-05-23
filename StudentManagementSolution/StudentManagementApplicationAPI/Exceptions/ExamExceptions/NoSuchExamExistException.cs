using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class NoSuchExamExistException : Exception
    {
        private string msg;
        public NoSuchExamExistException()
        {
            msg = "No Such Exam Record Found in the Database";
        }

        public NoSuchExamExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}