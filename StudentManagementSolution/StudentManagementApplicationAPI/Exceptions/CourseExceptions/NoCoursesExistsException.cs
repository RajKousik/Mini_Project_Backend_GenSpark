using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class NoCoursesExistsException : Exception
    {
        private string msg;
        public NoCoursesExistsException()
        {
            msg = "No Courses Found in the database";
        }

        public NoCoursesExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}