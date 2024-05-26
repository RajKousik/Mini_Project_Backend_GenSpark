using AutoMapper;
using Easy_Password_Validator;
using StudentManagementApplicationAPI.Exceptions.CommonExceptions;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementApplicationAPI.Services
{
    #region Summary
    /// <summary>
    /// Service for handling student authentication, including registration and login.
    /// </summary>
    #endregion
    public class StudentAuthService : IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO>, IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO>
    {
        #region Private Fields

        private readonly ITokenService _tokenService;
        private readonly IRepository<int, Student> _studentRepo;
        private readonly IRepository<int, Department> _departmentRepo;
        private readonly IMapper _mapper;
        private readonly PasswordValidatorService _passwordValidatorService;

        private readonly bool AllowPasswordValidation;

        #endregion

        #region Constructor
        #region Summary
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentAuthService"/> class.
        /// </summary>
        /// <param name="tokenService">The token service for generating JWT tokens.</param>
        /// <param name="studentRepo">The repository for student data.</param>
        /// <param name="mapper">The mapper for transforming DTOs and models.</param>
        /// <param name="departmentRepo">The repository for department data.</param>
        /// <param name="passwordValidatorService">The passwoord validator service.</param>
        #endregion
        public StudentAuthService(ITokenService tokenService, IRepository<int, Student> studentRepo, 
            IMapper mapper, IRepository<int, Department> departmentRepo,
            PasswordValidatorService passwordValidatorService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _studentRepo = studentRepo;
            _mapper = mapper;
            _departmentRepo = departmentRepo;
            _passwordValidatorService = passwordValidatorService;
            bool.TryParse(configuration.GetSection("AllowPasswordValidation").Value, out bool allowPasswordValidation);
            AllowPasswordValidation = allowPasswordValidation;
        }

        #endregion

        #region Public Methods

        #region Summary
        /// <summary>
        /// Logs in a student and generates a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="dto">The login data transfer object containing email and password.</param>
        /// <returns>A <see cref="StudentLoginReturnDTO"/> containing the login result and token.</returns>
        /// <exception cref="UnauthorizedUserException">Thrown when the credentials are invalid or the account is not activated.</exception>
        #endregion
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

        #region Summary
        /// <summary>
        /// Registers a new student.
        /// </summary>
        /// <param name="dto">The registration data transfer object containing student details.</param>
        /// <param name="type">The role type of the student.</param>
        /// <returns>A <see cref="StudentRegisterReturnDTO"/> containing the registered student's details.</returns>
        /// <exception cref="CannotAddStudentToAdminDepartmentException">Thrown when attempting to add a student to the admin department.</exception>
        /// <exception cref="UnableToAddStudentException">Thrown when the student could not be added to the database.</exception>
        /// <exception cref="NoSuchDepartmentExistException">Thrown when the specified department does not exist.</exception>
        /// <exception cref="DuplicateEmailException">Thrown when the email is already registered.</exception>
        /// <exception cref="UnableToRegisterException">Thrown when an unknown error occurs during registration.</exception>
        #endregion
        public async Task<StudentRegisterReturnDTO> Register(StudentRegisterDTO dto, RoleType type)
        {
            Student student;
            try
            {
                await ValidateEmail(dto.Email);
                await ValidateDepartment(dto.DepartmentId);

                if (await IsAdminDepartment(dto.DepartmentId))
                {
                    throw new CannotAddStudentToAdminDepartmentException();
                }

                #region Password Validation
                if (AllowPasswordValidation)
                {
                    var isPasswordValid = _passwordValidatorService.TestAndScore(dto.Password);
                    Debug.WriteLine($"Password score {_passwordValidatorService.Score}, {_passwordValidatorService.FailureMessages} ");
                    var failureMessages = string.Join(", ", _passwordValidatorService.FailureMessages);
                    if (!isPasswordValid)
                    {
                        Debug.WriteLine("Invalid password");
                        throw new InvalidPasswordException(failureMessages);
                    }
                }
                #endregion

                student = _mapper.Map<Student>(dto);

                HMACSHA512 hMACSHA = new HMACSHA512();
                student.PasswordHashKey = hMACSHA.Key;
                student.HashedPassword = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
                student.Status = ActivationStatus.Inactive;

                var addedStudent = await _studentRepo.Add(student);

                StudentRegisterReturnDTO studentRegisterReturnDTO = _mapper.Map<StudentRegisterReturnDTO>(addedStudent);

                return studentRegisterReturnDTO;
            }
            catch (CannotAddStudentToAdminDepartmentException ex)
            {
                throw new CannotAddStudentToAdminDepartmentException(ex.Message);
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
            catch (InvalidPasswordException ex)
            {
                throw new InvalidPasswordException(ex.Message);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new UnableToRegisterException(message);
            }
        }

        #endregion

        #region Private Methods

        #region Summary
        /// <summary>
        /// Validates if the email already exists in the database.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="DuplicateEmailException">Thrown when the email is already registered.</exception>
        #endregion

        /// <summary>
        /// Checks whether the department id is admin department or not
        /// </summary>
        /// <param name="departmentId">The department id to be checked</param>
        /// <returns>True, if it is admin department, else false</returns>
        private async Task<bool> IsAdminDepartment(int departmentId)
        {
            var department = await _departmentRepo.GetById(departmentId);
            return department != null && department.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether the email is valid or not
        /// </summary>
        /// <param name="email">The email to be validated</param>
        /// <returns></returns>
        /// <exception cref="DuplicateEmailException">DuplicateEmailException</exception>
        private async Task ValidateEmail(string email)
        {
            var emailExists = await FindIfEmailAlreadyExists(email);
            if (emailExists != null)
            {
                throw new DuplicateEmailException("Email id is already registered");
            }
        }

        #region Summary
        /// <summary>
        /// Validates if the department exists in the database.
        /// </summary>
        /// <param name="departmentId">The department ID to validate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CannotAddStudentToAdminDepartmentException">Thrown when attempting to add a student to the admin department.</exception>
        /// <exception cref="NoSuchDepartmentExistException">Thrown when the specified department does not exist.</exception>
        #endregion
        private async Task ValidateDepartment(int departmentId)
        {

            var departmentExists = await _departmentRepo.GetById(departmentId);
            if (departmentExists == null)
            {
                throw new NoSuchDepartmentExistException($"Department does not exist with id {departmentId}");
            }

            if (departmentExists.Name.ToLower().Contains("Admin".ToLower()))
            {
                throw new CannotAddStudentToAdminDepartmentException();
            }
        }

        #region Summary
        /// <summary>
        /// Finds if an email already exists in the student repository.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>A <see cref="Student"/> object if found, otherwise null.</returns>
        #endregion
        private async Task<Student> FindIfEmailAlreadyExists(string email)
        {
            return (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
        }

        #region Summary
        /// <summary>
        /// Compares two passwords to check if they are the same.
        /// </summary>
        /// <param name="encryptedPassword">The encrypted password.</param>
        /// <param name="userPassword">The stored user password.</param>
        /// <returns>True if the passwords match, otherwise false.</returns>
        #endregion
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

        #endregion
    }

}
