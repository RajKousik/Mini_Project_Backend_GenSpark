using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationRepository
{
    [Serializable]
    public class UnableToDeleteCourseRegistrationException : Exception
    {
        private string msg;
        public UnableToDeleteCourseRegistrationException()
        {
            msg = "Something went wrong while deleting a course registration record";
        }

        public UnableToDeleteCourseRegistrationException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}