using Microsoft.VisualBasic;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class CourseRegistrationAlreadyApprovedException : Exception
    {
        private string msg;
        public CourseRegistrationAlreadyApprovedException()
        {
            msg = "The course registration already approved";
        }

        public CourseRegistrationAlreadyApprovedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}