using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class UnableToUpdateGradeException : Exception
    {
        private string msg;
        public UnableToUpdateGradeException()
        {
            msg = "Something went wrong while updating a Grade";
        }

        public UnableToUpdateGradeException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}