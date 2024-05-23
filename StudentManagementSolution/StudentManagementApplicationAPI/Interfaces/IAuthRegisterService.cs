using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Interfaces
{
    public interface IAuthRegisterService<T, K> where T : class where K : class
    {
        public Task<T> Register(K dto, RoleType role = RoleType.Student);
    }
}
