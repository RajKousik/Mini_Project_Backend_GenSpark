using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class NoSuchCourseExistException : Exception
    {
        private string msg;
        public NoSuchCourseExistException()
        {
            msg = "No Such Course Found in the Database";
        }

        public NoSuchCourseExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}