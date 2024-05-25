using System.Globalization;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Services
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