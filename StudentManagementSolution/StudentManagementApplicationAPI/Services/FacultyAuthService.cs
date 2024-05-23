using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementApplicationAPI.Services
{
    public class FacultyAuthService : IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO>, IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO>
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<int, Faculty> _facultyRepo;
        private readonly IRepository<int, Department> _departmentRepo;
        private readonly IMapper _mapper;

        public FacultyAuthService(ITokenService tokenService, IRepository<int, Faculty> facultyRepo, IMapper mapper, IRepository<int, Department> departmentRepo)
        {
            _tokenService = tokenService;
            _facultyRepo = facultyRepo;
            _mapper = mapper;
            _departmentRepo = departmentRepo;
        }

        public async Task<FacultyLoginReturnDTO> Login(FacultyLoginDTO dto)
        {
            try
            {
                var facultyInDB = await FindIfEmailAlreadyExists(dto.Email);
                if (facultyInDB == null)
                {
                    throw new UnauthorizedUserException("Invalid username or password");
                }
                HMACSHA512 hMACSHA = new HMACSHA512(facultyInDB.PasswordHashKey);
                var encryptedPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
                bool isPasswordSame = ComparePassword(encryptedPass, facultyInDB.HashedPassword);
                if (isPasswordSame)
                {
                    if (facultyInDB.Status == ActivationStatus.Active)
                    {
                        FacultyLoginReturnDTO loginReturnDTO = new FacultyLoginReturnDTO()
                        {
                            Email = dto.Email,
                            FacultyId = facultyInDB.FacultyId,
                            Token = _tokenService.GenerateFacultyToken(facultyInDB),
                        };
                        return loginReturnDTO;
                    }
                    throw new UserNotActivatedException("Your account is not activated");
                }
            }
            catch (UserNotActivatedException ex)
            {
                throw new UserNotActivatedException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedUserException("Invalid username or password");
            }
            throw new UnauthorizedUserException("Invalid username or password");
        }

        private bool ComparePassword(byte[] encryptedPassword, byte[] userPassword)
        {
            if (encryptedPassword.Length != userPassword.Length)
            {
                return false;
            }
            for (int i = 0; i < encryptedPassword.Length; i++)
            {
                if (encryptedPassword[i] != userPassword[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<FacultyRegisterReturnDTO> Register(FacultyRegisterDTO dto, RoleType role)
        {
            Faculty faculty;
            try
            {
                var emailExists = await FindIfEmailAlreadyExists(dto.Email);

                if (emailExists != null)
                    throw new DuplicateEmailException("Email id is already registered");

                if (dto.DepartmentId != null || role != RoleType.Admin)
                { 
                    var departmentExists = await _departmentRepo.GetById((int)dto.DepartmentId);

                    if (departmentExists == null)
                    {
                        throw new NoSuchDepartmentExistException($"Department does not exist with id {dto.DepartmentId}");
                    }
                }

                if(role == RoleType.Admin)
                {
                    dto.DepartmentId = 2;
                }

                faculty = _mapper.Map<Faculty>(dto);

                HMACSHA512 hMACSHA = new HMACSHA512();
                faculty.PasswordHashKey = hMACSHA.Key;
                faculty.HashedPassword = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
                faculty.Status = ActivationStatus.Inactive;
                faculty.Role = role;

                var addedFaculty = await _facultyRepo.Add(faculty);

                FacultyRegisterReturnDTO facultyRegisterReturnDTO = _mapper.Map<FacultyRegisterReturnDTO>(addedFaculty);

                return facultyRegisterReturnDTO;
            }
            catch (UnableToAddFacultyException ex)
            {
                throw new UnableToAddFacultyException(ex.Message);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (DuplicateEmailException ex)
            {
                throw new DuplicateEmailException(ex.Message);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new UnableToRegisterException(message);
            }
        }

        private async Task<Faculty> FindIfEmailAlreadyExists(string email)
        {
            return (await _facultyRepo.GetAll()).FirstOrDefault(f => f.Email == email);
        }
    }

}
