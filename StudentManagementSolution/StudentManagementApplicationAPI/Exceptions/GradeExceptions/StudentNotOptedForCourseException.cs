using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class StudentNotOptedForCourseException : Exception
    {
        private string msg;
        public StudentNotOptedForCourseException()
        {
            msg = "Student Not Opted for this Course!";
        }

        public StudentNotOptedForCourseException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}