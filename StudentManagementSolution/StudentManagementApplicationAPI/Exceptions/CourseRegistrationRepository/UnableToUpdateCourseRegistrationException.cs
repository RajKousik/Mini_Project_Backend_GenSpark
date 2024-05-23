using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationRepository
{
    [Serializable]
    public class UnableToUpdateCourseRegistrationException : Exception
    {
        private string msg;
        public UnableToUpdateCourseRegistrationException()
        {
            msg = "Something went wrong while updating a course registration record";
        }

        public UnableToUpdateCourseRegistrationException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}