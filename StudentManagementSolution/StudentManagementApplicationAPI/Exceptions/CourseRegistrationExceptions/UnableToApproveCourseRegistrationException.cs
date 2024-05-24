using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class UnableToApproveCourseRegistrationException : Exception
    {
        private string msg;
        public UnableToApproveCourseRegistrationException()
        {
            msg = "Something went wrong! Unable to approve now";
        }

        public UnableToApproveCourseRegistrationException(string message) : base(message)
        {
            msg = Message;
        }

        public override string Message => msg;
    }
}