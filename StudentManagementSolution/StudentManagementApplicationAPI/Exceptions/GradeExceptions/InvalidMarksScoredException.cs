using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class InvalidMarksScoredException : Exception
    {
        private string msg;
        public InvalidMarksScoredException()
        {
            msg = "Marks scored cannot be greater than the total mark of the exam";
        }

        public InvalidMarksScoredException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}