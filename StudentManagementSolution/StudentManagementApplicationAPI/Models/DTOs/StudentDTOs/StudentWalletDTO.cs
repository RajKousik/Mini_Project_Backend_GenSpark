using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentDTOs
{
    public class StudentWalletDTO
    {
        public int StudentId { get; set; }
        public double RechargeAmount { get; set; }

        public string Password { get; set; }
    }
}