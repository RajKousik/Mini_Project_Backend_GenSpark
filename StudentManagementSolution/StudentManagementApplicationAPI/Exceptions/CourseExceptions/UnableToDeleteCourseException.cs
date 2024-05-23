using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class UnableToDeleteCourseException : Exception
    {
        private string msg;
        public UnableToDeleteCourseException()
        {
            msg = "Something went wrong while deleting a course record";
        }

        public UnableToDeleteCourseException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}