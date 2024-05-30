using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentDTOs
{
    public class StudentWalletDTO
    {
        public int StudentId { get; set; }
        [Range(1, Double.MaxValue)]
        public double RechargeAmount { get; set; }
    }
}