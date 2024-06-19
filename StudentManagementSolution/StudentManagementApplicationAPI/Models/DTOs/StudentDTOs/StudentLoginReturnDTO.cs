using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentDTOs
{
    public class StudentLoginReturnDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int StudentRollNo { get; set; }

        public string Token  { get; set; }

        public string Role { get; set; }
    }
}
