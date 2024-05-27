using StudentManagementApplicationAPI.Models.DTOs.ExamDTOs;

namespace StudentManagementApplicationAPI.Interfaces.Service
{
    public interface IExamService
    {
        public Task<ExamReturnDTO> AddExam(ExamDTO examDTO);
        public Task<IEnumerable<ExamReturnDTO>> GetAllExams();
        public Task<ExamReturnDTO> GetExamById(int examId);
        public Task<ExamReturnDTO> UpdateExam(int examId, ExamDTO examDTO);
        public Task<ExamReturnDTO> DeleteExam(int examId);
        public Task<IEnumerable<ExamReturnDTO>> GetExamsByDate(DateTime date);
        public Task<IEnumerable<ExamReturnDTO>> GetUpcomingExams(int days);
        public Task<IEnumerable<ExamReturnDTO>> GetOfflineExams();
        public Task<IEnumerable<ExamReturnDTO>> GetOnlineExams();

    }
}
