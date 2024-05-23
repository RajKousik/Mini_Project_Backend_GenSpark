using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class UnableToAddCourseException : Exception
    {
        private string msg;
        public UnableToAddCourseException()
        {
            msg = "Something went wrong while adding a course record";
        }

        public UnableToAddCourseException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}