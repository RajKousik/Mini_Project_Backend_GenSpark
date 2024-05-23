using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.FacultyExceptions
{
    [Serializable]
    public class NoFacultiesExistsException : Exception
    {
        private string msg;
        public NoFacultiesExistsException()
        {
            msg = "No Faculties Found in the database";
        }

        public NoFacultiesExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}