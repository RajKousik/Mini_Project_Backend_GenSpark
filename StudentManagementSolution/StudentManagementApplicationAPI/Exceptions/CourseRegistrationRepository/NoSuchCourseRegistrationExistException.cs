using StudentManagementApplicationAPI.Models.Db_Models;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseRegistrationRepository
{
    [Serializable]
    public class NoSuchCourseRegistrationExistException : Exception
    {
        private string msg;
        public NoSuchCourseRegistrationExistException()
        {
            msg = "No Such Course Registration Found in the Database";
        }

        public NoSuchCourseRegistrationExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}