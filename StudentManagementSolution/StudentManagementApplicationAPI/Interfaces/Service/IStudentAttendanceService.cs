using StudentManagementApplicationAPI.Models.DTOs.StudentAttendanceDTOs;
using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Interfaces.Service
{
    public interface IStudentAttendanceService
    {
        public Task<AttendanceReturnDTO> MarkAttendance(AttendanceDTO attendanceDTO);
        public Task<AttendanceReturnDTO> UpdateAttendance(int attendanceId, string attendanceStatus);
        public Task<AttendanceReturnDTO> DeleteAttendance(int attendanceId);
        public Task<AttendanceReturnDTO> GetAttendance(int attendanceId);
        public Task<IEnumerable<AttendanceReturnDTO>> GetAllAttendanceRecords();
        public Task<IEnumerable<AttendanceReturnDTO>> GetStudentAttendanceRecords(int studentId);
        public Task<IEnumerable<AttendanceReturnDTO>> GetCourseAttendanceRecords(int courseId);
        public Task<IEnumerable<AttendancePercentageDTO>> GetStudentAttendancePercentage(int studentId);
    }
}
