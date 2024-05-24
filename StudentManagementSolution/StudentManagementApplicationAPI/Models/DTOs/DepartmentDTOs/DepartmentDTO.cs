using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs
{
    public class DepartmentDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int HeadId { get; set; }
    }
}
