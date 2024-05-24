using AutoMapper;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;

namespace StudentManagementApplicationAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Student
            CreateMap<Student, StudentRegisterDTO>().ReverseMap();
            CreateMap<Student, StudentLoginDTO>().ReverseMap();
            CreateMap<Student, StudentLoginReturnDTO>().ReverseMap();
            CreateMap<Student, StudentRegisterReturnDTO>().ReverseMap();
            CreateMap<StudentRegisterDTO, StudentLoginDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();
            #endregion

            #region Faculty
            CreateMap<Faculty, FacultyRegisterDTO>().ReverseMap();
            CreateMap<Faculty, FacultyLoginDTO>().ReverseMap();
            CreateMap<Faculty, FacultyLoginReturnDTO>().ReverseMap();
            CreateMap<Faculty, FacultyRegisterReturnDTO>().ReverseMap();
            CreateMap<FacultyRegisterDTO, FacultyLoginDTO>().ReverseMap();
            CreateMap<Faculty, FacultyDTO>().ReverseMap();
            #endregion

            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Department, DepartmentReturnDTO>().ReverseMap();
            CreateMap<DepartmentReturnDTO, DepartmentDTO>().ReverseMap();
        }
    }
}
