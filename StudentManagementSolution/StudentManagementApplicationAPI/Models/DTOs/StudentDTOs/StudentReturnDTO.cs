﻿using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Models.DTOs.StudentDTOs
{
    public class StudentReturnDTO
    {
        public string Name { get; set; }

        public ActivationStatus Status { get; set; }
        public int StudentRollNo { get; set; }
        public string Email { get; set; }

        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int DepartmentId { get; set; }
    }
}
