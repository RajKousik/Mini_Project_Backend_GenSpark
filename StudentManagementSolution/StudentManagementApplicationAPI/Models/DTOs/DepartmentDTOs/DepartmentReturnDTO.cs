using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs
{
    public class DepartmentReturnDTO
    {
        public int DeptId { get; set; }
        public string Name { get; set; }

        public int HeadId { get; set; }
    }
}
