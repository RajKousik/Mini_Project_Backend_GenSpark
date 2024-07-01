using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class CourseRegistrationAlreadyRejectedException : Exception
    {
        private string msg;
        public CourseRegistrationAlreadyRejectedException()
        {
            msg = "The course registration already rejected";
        }

        public CourseRegistrationAlreadyRejectedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}