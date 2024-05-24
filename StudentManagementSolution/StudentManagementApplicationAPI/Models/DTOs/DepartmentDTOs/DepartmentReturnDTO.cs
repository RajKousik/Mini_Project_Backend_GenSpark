using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs
{
    public class DepartmentReturnDTO
    {
        public int DeptId;
        public string Name { get; set; }

        public int HeadId { get; set; }
    }
}
