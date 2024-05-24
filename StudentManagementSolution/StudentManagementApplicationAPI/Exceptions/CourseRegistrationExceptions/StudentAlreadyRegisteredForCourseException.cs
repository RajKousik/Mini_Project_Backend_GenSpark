using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class StudentAlreadyRegisteredForCourseException : Exception
    {
        private string msg;
        public StudentAlreadyRegisteredForCourseException()
        {
            msg = "Student has already registered for this course";
        }

        public StudentAlreadyRegisteredForCourseException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}