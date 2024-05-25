using AutoMapper;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.CourseDTOs;
using StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs;
using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;
using StudentManagementApplicationAPI.Models.DTOs.ExamDTOs;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.DTOs.GradeDTOs;
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

            #region Department
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Department, DepartmentReturnDTO>().ReverseMap();
            CreateMap<DepartmentReturnDTO, DepartmentDTO>().ReverseMap();
            #endregion

            #region Course
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Course, CourseReturnDTO>().ReverseMap();
            CreateMap<Course, CourseUpdateDTO>().ReverseMap();
            CreateMap<CourseReturnDTO, CourseDTO>().ReverseMap();
            CreateMap<CourseUpdateDTO, CourseDTO>().ReverseMap();
            CreateMap<CourseUpdateDTO, CourseReturnDTO>().ReverseMap();
            #endregion

            #region Course Registration
            CreateMap<CourseRegistration, CourseRegistrationDTO>().ReverseMap();
            CreateMap<CourseRegistration, CourseRegistrationReturnDTO>().ReverseMap();
            CreateMap<CourseRegistration, CourseRegistrationAddDTO>().ReverseMap();
            CreateMap<CourseRegistrationReturnDTO, CourseRegistrationAddDTO>().ReverseMap();
            CreateMap<CourseRegistrationDTO, CourseRegistrationAddDTO>().ReverseMap();
            CreateMap<CourseRegistrationDTO, CourseRegistrationReturnDTO>().ReverseMap();

            #endregion

            #region Exams
            CreateMap<Exam, ExamDTO>().ReverseMap();
            CreateMap<Exam, ExamReturnDTO>().ReverseMap();
            CreateMap<ExamDTO, ExamReturnDTO>().ReverseMap();
            #endregion

            #region Grade
            CreateMap<Grade, GradeDTO>().ReverseMap();
            CreateMap<Grade, GradeReturnDTO>().ReverseMap();
            CreateMap<Grade, GradeUpdateDTO>().ReverseMap();
            CreateMap<GradeReturnDTO, GradeUpdateDTO>().ReverseMap();
            CreateMap<GradeDTO, GradeUpdateDTO>().ReverseMap();
            CreateMap<GradeDTO, GradeReturnDTO>().ReverseMap();

            #endregion
        }
    }
}
