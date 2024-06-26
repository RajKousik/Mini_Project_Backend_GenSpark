﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Student
    {
        [Key]
        public int StudentRollNo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public int Age => DateTime.Now.Year - DOB.Year;

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        public byte[] PasswordHashKey { get; set; }

        [Required]
        public byte[] HashedPassword { get; set; }

        [Required]
        public ActivationStatus Status { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public double EWallet { get; set; }
    }
}
