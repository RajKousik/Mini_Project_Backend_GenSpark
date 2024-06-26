﻿using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentDTOs
{
    public class StudentLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
