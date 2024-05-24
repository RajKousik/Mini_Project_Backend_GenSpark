using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class UnableToAddCourseRegistrationException : Exception
    {
        private string msg;
        public UnableToAddCourseRegistrationException()
        {
            msg = "Something went wrong while adding a course registration record";
        }

        public UnableToAddCourseRegistrationException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}