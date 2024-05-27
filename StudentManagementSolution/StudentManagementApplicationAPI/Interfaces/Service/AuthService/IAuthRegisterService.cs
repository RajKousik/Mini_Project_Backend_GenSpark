using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Interfaces.Service.AuthService
{
    public interface IAuthRegisterService<T, K> where T : class where K : class
    {
        public Task<T> Register(K dto, RoleType role = RoleType.Student);
    }
}
