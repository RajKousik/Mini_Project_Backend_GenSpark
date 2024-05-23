using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class NoExamsExistsException : Exception
    {
        private string msg;
        public NoExamsExistsException()
        {
            msg = "No Exam Records Found in the database";
        }

        public NoExamsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}