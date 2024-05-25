using StudentManagementApplicationAPI.Models.DTOs.GradeDTOs;

namespace StudentManagementApplicationAPI.Interfaces
{
    public interface IGradeService
    {
        public Task<GradeReturnDTO> AddGrade(GradeDTO gradeDTO);
        public Task<IEnumerable<GradeReturnDTO>> GetAllGrades();
        public Task<GradeReturnDTO> GetGradeById(int gradeId);
        public Task<GradeReturnDTO> UpdateGrade(int gradeId, GradeUpdateDTO gradeUpdateDTO);
        public Task<GradeReturnDTO> DeleteGrade(int gradeId);
        public Task<IEnumerable<GradeReturnDTO>> GetStudentGrades(int studentId);
        public Task<IEnumerable<GradeReturnDTO>> GetCourseGrades(int courseId);
    }
}
