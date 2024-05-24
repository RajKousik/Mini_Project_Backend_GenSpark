using StudentManagementApplicationAPI.Models.Db_Models;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.CourseExceptions
{
    [Serializable]
    public class NoCoursesExistForFacultyException : Exception
    {
        private string msg;
        public NoCoursesExistForFacultyException()
        {
            msg = "No courses exist for faculty with ID";
        }

        public NoCoursesExistForFacultyException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}