using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class NoGradeRecordsExistsException : Exception
    {
        private string msg;
        public NoGradeRecordsExistsException()
        {
            msg = "No Grade Records Found in the database";
        }

        public NoGradeRecordsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}