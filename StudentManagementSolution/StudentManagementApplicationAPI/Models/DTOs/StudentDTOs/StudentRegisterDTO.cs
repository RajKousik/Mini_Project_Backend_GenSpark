using System.ComponentModel.DataAnnotations;
#nullable disable
namespace StudentManagementApplicationAPI.Models.DTOs.StudentDTOs
{
    public class StudentRegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }

}
