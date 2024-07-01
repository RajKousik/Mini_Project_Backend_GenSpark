using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;

namespace StudentManagementApplicationAPI.Interfaces.Service
{
    public interface IDepartmentService
    {
        public Task<DepartmentReturnDTO> AddDepartment(DepartmentDTO departmentDTO);
        public Task<DepartmentDTO> DeleteDepartment(int departmentId);
        public Task<DepartmentReturnDTO> GetDepartmentById(int departmentId);
        public Task<IEnumerable<DepartmentReturnDTO>> GetAllDepartments();
        public Task<DepartmentDTO> ChangeDepartmentHead(int departmentId, int newHeadDepartmentId);
    }
}
