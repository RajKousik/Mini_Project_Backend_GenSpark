using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementApplicationAPI.Services
{
    public class StudentAuthService : IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO>, IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO>
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<int, Student> _studentRepo;
        private readonly IRepository<int, Department> _departmentRepo;
        private readonly IMapper _mapper;

        public StudentAuthService(ITokenService tokenService, IRepository<int, Student> studentRepo, IMapper mapper, IRepository<int, Department> departmentRepo)
        {
            _tokenService = tokenService;
            _studentRepo = studentRepo;
            _mapper = mapper;
            _departmentRepo = departmentRepo;
        }

        public async Task<StudentLoginReturnDTO> Login(StudentLoginDTO dto)
        {
            try
            {
                var studentInDB = await FindIfEmailAlreadyExists(dto.Email);
                if (studentInDB == null)
                {
                    throw new UnauthorizedUserException("Invalid username or password");
                }
                HMACSHA512 hMACSHA = new HMACSHA512(studentInDB.PasswordHashKey);
                var encryptedPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
                bool isPasswordSame = ComparePassword(encryptedPass, studentInDB.HashedPassword);
                if (isPasswordSame)
                {
                    if (studentInDB.Status == ActivationStatus.Active)
                    {
                        StudentLoginReturnDTO loginReturnDTO = new StudentLoginReturnDTO()
                        {
                            Email = dto.Email,
                            StudentRollNo = studentInDB.StudentRollNo,
                            Token = _tokenService.GenerateStudentToken(studentInDB),
                        };
                        return loginReturnDTO;
                    }
                    throw new UserNotActivatedException("Your account is not activated");
                }
            }
            catch(UserNotActivatedException ex)
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
            if(encryptedPassword.Length != userPassword.Length)
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

        public async Task<StudentRegisterReturnDTO> Register(StudentRegisterDTO dto, RoleType type)
        {
            Student student;
            try
            {

                var emailExists = await FindIfEmailAlreadyExists(dto.Email);

                if (emailExists != null)
                    throw new DuplicateEmailException("Email id is already registered");

                var departmentExists = await _departmentRepo.GetById(dto.DepartmentId);

                if(departmentExists == null)
                {
                    throw new NoSuchDepartmentExistException($"Department does not exist with id {dto.DepartmentId}");
                }

                student = _mapper.Map<Student>(dto);

                HMACSHA512 hMACSHA = new HMACSHA512();
                student.PasswordHashKey = hMACSHA.Key;
                student.HashedPassword = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
                student.Status = ActivationStatus.Inactive;

                

                var addedStudent = await _studentRepo.Add(student);

                StudentRegisterReturnDTO studentRegisterReturnDTO = _mapper.Map<StudentRegisterReturnDTO>(addedStudent);

                return studentRegisterReturnDTO;
            }
            catch (UnableToAddStudentException ex)
            {
                throw new UnableToAddStudentException(ex.Message);
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

        private async Task<Student> FindIfEmailAlreadyExists(string email)
        {
            return (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
        }
    }
}
