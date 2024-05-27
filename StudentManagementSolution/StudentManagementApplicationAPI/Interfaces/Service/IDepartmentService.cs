using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;

namespace StudentManagementApplicationAPI.Interfaces.Service
{
    public interface IDepartmentService
    {
        public Task<DepartmentReturnDTO> AddDepartment(DepartmentDTO departmentDTO);
        public Task<DepartmentDTO> DeleteDepartment(int departmentId);
        public Task<DepartmentDTO> GetDepartmentById(int departmentId);
        public Task<IEnumerable<DepartmentDTO>> GetAllDepartments();
        public Task<DepartmentDTO> ChangeDepartmentHead(int departmentId, int newHeadDepartmentId);
    }
}
