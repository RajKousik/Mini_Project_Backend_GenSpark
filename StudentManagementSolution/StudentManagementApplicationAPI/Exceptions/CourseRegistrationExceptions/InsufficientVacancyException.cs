using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions
{
    [Serializable]
    public class InsufficientVacancyException : Exception
    {
        private string msg;
        public InsufficientVacancyException()
        {
            msg = "Not enough vacancy available for the course";
        }

        public InsufficientVacancyException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}