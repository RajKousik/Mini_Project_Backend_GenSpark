﻿using StudentManagementApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs
{
    public class FacultyDTO
    {

        public int FacultyId { get; set; }

        public string Email { get; set; }

        public RoleType Role { get; set; }
        public ActivationStatus Status { get; set; }

        public int? DepartmentId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "DOB is required")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Mobile no. is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
