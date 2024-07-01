using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services.Faculty_Service
{
    public class FacultyService : IFacultyService
    {
        #region Private Fields

        private readonly IRepository<int, Faculty> _facultyRepository;
        private readonly IRepository<int, Department> _departmentRepository;
        private readonly ILogger<FacultyService> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public FacultyService(IRepository<int, Faculty> facultyRepository, IMapper mapper, 
            IRepository<int, Department> departmentRepository, ILogger<FacultyService> logger)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the details of a faculty member.
        /// </summary>
        /// <param name="dto">The data transfer object containing updated faculty details.</param>
        /// <param name="email">The email of the faculty to update.</param>
        /// <returns>The updated faculty data transfer object.</returns>
        public async Task<FacultyDTO> UpdateFaculty(FacultyUpdateDTO dto, string email)
        {
            try
            {
                var facultyInDB = (await _facultyRepository.GetAll()).FirstOrDefault(f => f.Email == email);
                if (facultyInDB == null)
                {
                    throw new NoSuchFacultyExistException($"Faculty with email {email} does not exist.");
                }

                if (!string.IsNullOrEmpty(dto.Name))
                    facultyInDB.Name = dto.Name;
                if (dto.DOB != default)
                    facultyInDB.DOB = dto.DOB;
                if (!string.IsNullOrEmpty(dto.Gender))
                    facultyInDB.Gender = dto.Gender;
                if (!string.IsNullOrEmpty(dto.Mobile))
                    facultyInDB.Mobile = dto.Mobile;
                if (!string.IsNullOrEmpty(dto.Address))
                    facultyInDB.Address = dto.Address;

                await _facultyRepository.Update(facultyInDB);
                return _mapper.Map<FacultyDTO>(facultyInDB);
            }
            catch(NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (UnableToUpdateFacultyException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateFacultyException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Something Went Wrong!");
            }
        }

        /// <summary>
        /// Deletes a faculty member.
        /// </summary>
        /// <param name="email">The email of the faculty to delete.</param>
        /// <returns>The deleted faculty data transfer object.</returns>
        public async Task<FacultyDTO> DeleteFaculty(string email)
        {
            try
            {
                var facultyInDB = (await _facultyRepository.GetAll()).FirstOrDefault(f => f.Email == email);
                if (facultyInDB == null)
                {
                    throw new NoSuchFacultyExistException($"Faculty with email {email} does not exist.");
                }

                await _facultyRepository.Delete(facultyInDB.FacultyId);
                return _mapper.Map<FacultyDTO>(facultyInDB);
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (UnableToDeleteFacultyException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToDeleteFacultyException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a faculty member by their ID.
        /// </summary>
        /// <param name="facultyId">The ID of the faculty.</param>
        /// <returns>The faculty data transfer object.</returns>
        public async Task<FacultyDTO> GetFacultyById(int facultyId)
        {
            try
            {
                var faculty = await _facultyRepository.GetById(facultyId);

                return _mapper.Map<FacultyDTO>(faculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a faculty member by their email.
        /// </summary>
        /// <param name="email">The email of the faculty.</param>
        /// <returns>The faculty data transfer object.</returns>
        public async Task<FacultyDTO> GetFacultyByEmail(string email)
        {
            try
            {
                var faculty = (await _facultyRepository.GetAll()).FirstOrDefault(f => f.Email == email);
                if (faculty == null)
                {
                    throw new NoSuchFacultyExistException($"Faculty with email {email} does not exist.");
                }

                return _mapper.Map<FacultyDTO>(faculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves faculty members by their name.
        /// </summary>
        /// <param name="name">The name of the faculty.</param>
        /// <returns>The list of faculty data transfer objects.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetFacultyByName(string name)
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).Where(f => f.Name.ToLower().Contains(name.ToLower())).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all faculty members.
        /// </summary>
        /// <returns>The list of all faculty data transfer objects.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetAll()
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all professors.
        /// </summary>
        /// <returns>The list of professor data transfer objects.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetProfessors()
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).Where(f => f.Role == RoleType.Proffesors).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all associate professors.
        /// </summary>
        /// <returns>The list of associate professor data transfer objects.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetAssociateProfessors()
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).Where(f => f.Role == RoleType.Associate_Proffesors).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all assistant professors.
        /// </summary>
        /// <returns>The list of assistant professor data transfer objects.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetAssistantProfessors()
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).Where(f => f.Role == RoleType.Assistant_Proffesors).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all heads of department.
        /// </summary>
        /// <returns>The list of head of department data transfer objects.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetHeadOfDepartment()
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).Where(f => f.Role == RoleType.Head_Of_Department).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves faculty members by department ID.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>The list of faculty data transfer objects in the specified department.</returns>
        public async Task<IEnumerable<FacultyDTO>> GetFacultiesByDepartment(int departmentId)
        {
            try
            {
                var faculties = (await _facultyRepository.GetAll()).Where(f => f.DepartmentId == departmentId).ToList();
                if (faculties.Count == 0)
                {
                    throw new NoFacultiesExistsException();
                }
                return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoFacultiesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<FacultyDTO> ChangeDepartment(int facultyId, int deptId)
        {
            try
            {
                // Check if the faculty exists
                var faculty = await _facultyRepository.GetById(facultyId);
                if (faculty == null)
                {
                    throw new NoSuchFacultyExistException($"Faculty with ID {facultyId} does not exist.");
                }

                // Check if the department exists
                var department = await _departmentRepository.GetById(deptId);


                // Check if the faculty is the head of any department
                if(faculty.Role == RoleType.Head_Of_Department)
                {
                    throw new UnableToUpdateDepartmentException($"Faculty with ID {facultyId} is the head of a department and cannot be changed.");
                }

                // Update the faculty's department
                faculty.DepartmentId = deptId;
                await _facultyRepository.Update(faculty);

                // Map and return the updated faculty DTO
                return _mapper.Map<FacultyDTO>(faculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (UnableToUpdateDepartmentException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateDepartmentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }


        #endregion
    }

}
