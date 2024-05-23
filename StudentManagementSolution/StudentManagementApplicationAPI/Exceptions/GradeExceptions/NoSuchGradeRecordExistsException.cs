using StudentManagementApplicationAPI.Models.Db_Models;
using System;
using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Exceptions.GradeExceptions
{
    [Serializable]
    public class NoSuchGradeRecordExistsException : Exception
    {
        private string msg;
        public NoSuchGradeRecordExistsException()
        {
            msg = "No Such Grade Record Found in the Database";
        }

        public NoSuchGradeRecordExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}