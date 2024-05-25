using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.ExamExceptions
{
    [Serializable]
    public class StudentAlreadyApprovedForCourseException : Exception
    {
        private string msg;
        public StudentAlreadyApprovedForCourseException()
        {
            msg = "Student is already approved for the course";
        }

        public StudentAlreadyApprovedForCourseException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}