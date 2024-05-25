using System.Globalization;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class InvalidGradeException : Exception
    {
        private string msg;
        public InvalidGradeException()
        {
            msg = "Invalid percentage calculation.";
        }

        public InvalidGradeException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}