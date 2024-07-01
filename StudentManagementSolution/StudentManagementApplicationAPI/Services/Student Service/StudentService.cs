using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.CommonExceptions;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using System.Security.Cryptography;
using System.Text;



namespace StudentManagementApplicationAPI.Services.Student_Service
{
    /// <summary>
    /// Service class to handle student-related operations.
    /// </summary>
    public class StudentService : IStudentService
    {
        #region Fields
        private readonly IRepository<int, Student> _studentRepo;
        private readonly IRepository<int, Department> _departmentRepo;
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentService"/> class.
        /// </summary>
        /// <param name="studentRepo">The student repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="departmentRepo">The department repository.</param>
        public StudentService(IRepository<int, Student> studentRepo, IMapper mapper,
            IRepository<int, Department> departmentRepo, ILogger<StudentService> logger)
        {
            _studentRepo = studentRepo;
            _mapper = mapper;
            _departmentRepo = departmentRepo;
            _logger = logger;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes a student by email.
        /// </summary>
        /// <param name="email">The email of the student.</param>
        /// <returns>The deleted student.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student does not exist.</exception>
        /// <exception cref="UnableToDeleteStudentException">Thrown when the student cannot be deleted.</exception>
        public async Task<StudentDTO> DeleteStudent(string email)
        {
            try
            {
                var student = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
                }

                await _studentRepo.Delete(student.StudentRollNo);
                return _mapper.Map<StudentDTO>(student);
            }
            catch (UnableToDeleteStudentException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToDeleteStudentException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets students by name.
        /// </summary>
        /// <param name="name">The name of the students.</param>
        /// <returns>A list of students.</returns>
        /// <exception cref="NoStudentsExistsException">Thrown when no students exist.</exception>

        public async Task<IEnumerable<StudentDTO>> GetStudentByName(string name)
        {
            try
            {
                var students = (await _studentRepo.GetAll()).Where(f => f.Name.ToLower().Contains(name.ToLower())).ToList();
                if (students.Count == 0)
                {
                    throw new NoStudentsExistsException();
                }
                return _mapper.Map<IEnumerable<StudentDTO>>(students);
            }
            catch (NoStudentsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoStudentsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets all students.
        /// </summary>
        /// <returns>A list of students.</returns>
        /// <exception cref="NoStudentsExistsException">Thrown when no students exist.</exception>
        public async Task<IEnumerable<StudentReturnDTO>> GetAllStudents()
        {
            try
            {
                var students = (await _studentRepo.GetAll()).ToList();
                if (students.Count == 0)
                {
                    throw new NoStudentsExistsException();
                }
                return _mapper.Map<IEnumerable<StudentReturnDTO>>(students);
            }
            catch (NoStudentsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoStudentsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets a student by email.
        /// </summary>
        /// <param name="email">The email of the student.</param>
        /// <returns>The student.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student does not exist.</exception>
        public async Task<StudentDTO> GetStudentByEmail(string email)
        {
            try
            {
                var student = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
                }

                return _mapper.Map<StudentDTO>(student);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets a student by roll number.
        /// </summary>
        /// <param name="studentRollNo">The roll number of the student.</param>
        /// <returns>The student.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student does not exist.</exception>
        public async Task<StudentReturnDTO> GetStudentById(int studentRollNo)
        {
            try
            {
                var student = await _studentRepo.GetById(studentRollNo);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with roll number {studentRollNo} does not exist.");
                }

                return _mapper.Map<StudentReturnDTO>(student);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets students by department.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>A list of students.</returns>
        /// <exception cref="NoSuchDepartmentExistException">Thrown when the department does not exist.</exception>
        /// <exception cref="NoStudentsExistsException">Thrown when no students exist in the department.</exception>
        public async Task<IEnumerable<StudentReturnDTO>> GetStudentsByDepartment(int departmentId)
        {
            try
            {
                var department = await _departmentRepo.GetById(departmentId);
                if (department == null)
                {
                    throw new NoSuchDepartmentExistException($"Department with id {departmentId} does not exist.");
                }

                var students = (await _studentRepo.GetAll()).Where(s => s.DepartmentId == departmentId).ToList();
                if (students.Count == 0)
                {
                    throw new NoStudentsExistsException($"No students in the department {departmentId}");
                }
                return _mapper.Map<IEnumerable<StudentReturnDTO>>(students);
            }
            catch (NoStudentsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoStudentsExistsException(ex.Message);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates a student by email.
        /// </summary>
        /// <param name="dto">The student DTO.</param>
        /// <param name="email">The email of the student.</param>
        /// <returns>The updated student.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student does not exist.</exception>
        /// <exception cref="NoSuchDepartmentExistException">Thrown when the department does not exist.</exception>
        /// <exception cref="CannotAddStudentToAdminDepartmentException">Thrown when trying to add a student to the Admin department.</exception>
        /// <exception cref="UnableToUpdateStudentException">Thrown when the student cannot be updated.</exception>
        public async Task<StudentDTO> UpdateStudent(StudentDTO dto, string email)
        {
            try
            {
                var studentInDB = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
                if (studentInDB == null)
                {
                    throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
                }

                if (!string.IsNullOrEmpty(dto.Name))
                {
                    studentInDB.Name = dto.Name;
                }

                if (dto.DOB != null)
                {
                    studentInDB.DOB = dto.DOB;
                }

                if (!string.IsNullOrEmpty(dto.Gender))
                {
                    studentInDB.Gender = dto.Gender;
                }

                if (!string.IsNullOrEmpty(dto.Mobile))
                {
                    studentInDB.Mobile = dto.Mobile;
                }

                if (!string.IsNullOrEmpty(dto.Address))
                {
                    studentInDB.Address = dto.Address;
                }

                var departmentExists = await _departmentRepo.GetById(dto.DepartmentId);
                if (departmentExists == null)
                {
                    throw new NoSuchDepartmentExistException();
                }
                if (departmentExists.Name.ToLower().Contains("Admin".ToLower()))
                {
                    throw new CannotAddStudentToAdminDepartmentException();
                }
                studentInDB.DepartmentId = dto.DepartmentId;
                await _studentRepo.Update(studentInDB);
                return _mapper.Map<StudentDTO>(studentInDB);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (CannotAddStudentToAdminDepartmentException ex)
            {
                _logger.LogError(ex.Message);
                throw new CannotAddStudentToAdminDepartmentException(ex.Message);
            }
            catch (UnableToUpdateStudentException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateStudentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentWalletReturnDTO> RechargeWallet(StudentWalletDTO studentWalletDTO)
        {
            try
            {
                var studentInDB = await _studentRepo.GetById(studentWalletDTO.StudentId);

                if (studentWalletDTO.RechargeAmount <= 0)
                {
                    throw new InvalidRechargeAmount("Invalid Recharge Amount");
                }

                HMACSHA512 hMACSHA = new HMACSHA512(studentInDB.PasswordHashKey);
                var encryptedPassword = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(studentWalletDTO.Password));
                bool isPasswordSame = ComparePassword(encryptedPassword, studentInDB.HashedPassword);

                if(!isPasswordSame)
                {
                    throw new InvalidPasswordException("Please enter correct password!");
                }

                studentInDB.EWallet += studentWalletDTO.RechargeAmount;

                var updatedStudent = await _studentRepo.Update(studentInDB);

                StudentWalletReturnDTO result = _mapper.Map<StudentWalletReturnDTO>(updatedStudent);
                result.StudentId = studentWalletDTO.StudentId;
                return result;

            }
            catch (InvalidRechargeAmount ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidRechargeAmount(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidPasswordException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
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

        public async Task<double> GetEWalletAmount(int studentRollNo)
        {
            try
            {
                var studentInDB = await _studentRepo.GetById(studentRollNo);

                if (studentInDB.EWallet <= 0)
                {
                    throw new InvalidRechargeAmount("Invalid Recharge Amount");
                }

                return studentInDB.EWallet;

            }
            catch (InvalidRechargeAmount ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidRechargeAmount(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }


}
