using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using System.Globalization;

namespace StudentManagementApplicationAPI.Interfaces.Service
{
    public interface IFacultyService
    {
        public Task<FacultyDTO> UpdateFaculty(FacultyDTO dto, string email);
        public Task<FacultyDTO> DeleteFaculty(string email);
        public Task<FacultyDTO> GetFacultyById(int facultyId);
        public Task<FacultyDTO> GetFacultyByEmail(string email);
        public Task<FacultyDTO> ChangeDepartment(int facultyId, int deptId);
        public Task<IEnumerable<FacultyDTO>> GetFacultyByName(string name);
        public Task<IEnumerable<FacultyDTO>> GetAll();
        public Task<IEnumerable<FacultyDTO>> GetProfessors();
        public Task<IEnumerable<FacultyDTO>> GetAssociateProfessors();
        public Task<IEnumerable<FacultyDTO>> GetAssistantProfessors();
        public Task<IEnumerable<FacultyDTO>> GetHeadOfDepartment();
        public Task<IEnumerable<FacultyDTO>> GetFacultiesByDepartment(int departmentId);

    }
}
