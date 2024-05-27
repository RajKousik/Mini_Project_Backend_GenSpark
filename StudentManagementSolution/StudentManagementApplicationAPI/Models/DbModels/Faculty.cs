using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using StudentManagementApplicationAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public int Age => DateTime.Now.Year - DOB.Year;

        [Required]
        public string Mobile { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHashKey { get; set; }

        [Required]
        public byte[] HashedPassword { get; set; }

        [Required]
        public ActivationStatus Status { get; set; }

        [Required]
        public RoleType Role { get; set; }

        [AllowNull]
        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
    }
}
