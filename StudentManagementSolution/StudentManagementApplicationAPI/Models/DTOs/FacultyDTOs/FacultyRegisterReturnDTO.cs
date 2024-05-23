namespace StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs
{
    public class FacultyRegisterReturnDTO
    {
        public int FacultyId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int DepartmentId { get; set; }
        public string Role { get; set; }
    }
}
