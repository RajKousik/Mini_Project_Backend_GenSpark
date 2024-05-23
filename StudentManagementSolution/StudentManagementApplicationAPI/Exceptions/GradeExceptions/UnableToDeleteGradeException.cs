using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class UnableToDeleteGradeException : Exception
    {
        private string msg;
        public UnableToDeleteGradeException()
        {
            msg = "Something went wrong while deleting a Grade";
        }

        public UnableToDeleteGradeException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}