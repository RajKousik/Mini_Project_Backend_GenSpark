﻿using AutoMapper;
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
        #region Private Fields

        private readonly ITokenService _tokenService;
        private readonly IRepository<int, Faculty> _facultyRepo;
        private readonly IRepository<int, Department> _departmentRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        #region Summary
        /// <summary>
        /// Initializes a new instance of the <see cref="FacultyAuthService"/> class.
        /// </summary>
        /// <param name="tokenService">The token service for generating JWT tokens.</param>
        /// <param name="facultyRepo">The repository for faculty data.</param>
        /// <param name="mapper">The mapper for transforming DTOs and models.</param>
        /// <param name="departmentRepo">The repository for department data.</param>
        #endregion
        public FacultyAuthService(ITokenService tokenService, IRepository<int, Faculty> facultyRepo, IMapper mapper, IRepository<int, Department> departmentRepo)
        {
            _tokenService = tokenService;
            _facultyRepo = facultyRepo;
            _mapper = mapper;
            _departmentRepo = departmentRepo;
        }

        #endregion

        #region Public Methods

        #region Summary
        /// <summary>
        /// Logs in a faculty member and generates a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="dto">The login data transfer object containing email and password.</param>
        /// <returns>A <see cref="FacultyLoginReturnDTO"/> containing the login result and token.</returns>
        /// <exception cref="UnauthorizedUserException">Thrown when the credentials are invalid or the account is not activated.</exception>
        #endregion
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

        #region Summary
        /// <summary>
        /// Registers a new faculty member.
        /// </summary>
        /// <param name="dto">The registration data transfer object containing faculty details.</param>
        /// <param name="type">The role type of the faculty.</param>
        /// <returns>A <see cref="FacultyRegisterReturnDTO"/> containing the registered faculty member's details.</returns>
        /// <exception cref="UnableToAddFacultyException">Thrown when the faculty member could not be added to the database.</exception>
        /// <exception cref="NoSuchDepartmentExistException">Thrown when the specified department does not exist.</exception>
        /// <exception cref="DuplicateEmailException">Thrown when the email is already registered.</exception>
        /// <exception cref="UnableToRegisterException">Thrown when an unknown error occurs during registration.</exception>
        #endregion
        public async Task<FacultyRegisterReturnDTO> Register(FacultyRegisterDTO dto, RoleType type)
        {
            Faculty faculty;
            try
            {
                var emailExists = await FindIfEmailAlreadyExists(dto.Email);

                if (emailExists != null)
                    throw new DuplicateEmailException("Email id is already registered");

                if (dto.DepartmentId != null || type != RoleType.Admin)
                {
                    var departmentExists = await _departmentRepo.GetById((int)dto.DepartmentId);

                    if (departmentExists == null)
                    {
                        throw new NoSuchDepartmentExistException($"Department does not exist with id {dto.DepartmentId}");
                    }
                }

                if (type == RoleType.Admin)
                {
                    dto.DepartmentId = 2;
                }

                faculty = _mapper.Map<Faculty>(dto);

                HMACSHA512 hMACSHA = new HMACSHA512();
                faculty.PasswordHashKey = hMACSHA.Key;
                faculty.HashedPassword = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
                faculty.Status = ActivationStatus.Inactive;
                faculty.Role = type;

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

        #endregion

        #region Private Methods

        #region Summary
        /// <summary>
        /// Finds if an email already exists in the faculty repository.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>A <see cref="Faculty"/> object if found, otherwise null.</returns>
        #endregion
        private async Task<Faculty> FindIfEmailAlreadyExists(string email)
        {
            return (await _facultyRepo.GetAll()).FirstOrDefault(f => f.Email == email);
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
