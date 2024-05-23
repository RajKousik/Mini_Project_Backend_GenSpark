using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs
{
    public class FacultyLoginReturnDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int FacultyId { get; set; }

        public string Token { get; set; }
    }
}
