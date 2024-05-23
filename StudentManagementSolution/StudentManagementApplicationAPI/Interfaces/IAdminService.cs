namespace StudentManagementApplicationAPI.Interfaces
{
    public interface IAdminService
    {
        public Task<string> ActivateStudent(string studentEmail);
        public Task<string> ActivateFaculty(string facultyEmail);

    }
}
