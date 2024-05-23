using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class UnableToAddGradeException : Exception
    {
        private string msg;
        public UnableToAddGradeException()
        {
            msg = "Something went wrong while adding a Grade";
        }

        public UnableToAddGradeException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}