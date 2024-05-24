using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class CourseAlreadyExistsException : Exception
    {
        private string msg;
        public CourseAlreadyExistsException()
        {
            msg = "Course with similiar Name already exists";
        }

        public CourseAlreadyExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}