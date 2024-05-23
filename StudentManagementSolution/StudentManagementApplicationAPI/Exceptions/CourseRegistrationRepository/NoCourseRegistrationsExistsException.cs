using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationRepository
{
    [Serializable]
    public class NoCourseRegistrationsExistsException : Exception
    {
        private string msg;
        public NoCourseRegistrationsExistsException()
        {
            msg = "No Courses Registrations Found in the database";
        }

        public NoCourseRegistrationsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}