using Microsoft.VisualBasic;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class InvalidExamTypeException : Exception
    {
        private string msg;
        public InvalidExamTypeException()
        {
            msg = "Invalid Exam Type, It should be either 'Online' or 'Offline'";
        }

        public InvalidExamTypeException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}