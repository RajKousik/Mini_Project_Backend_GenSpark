using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class UnableToUpdateCourseException : Exception
    {
        private string msg;
        public UnableToUpdateCourseException()
        {
            msg = "Something went wrong while updating a course record";
        }

        public UnableToUpdateCourseException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}