using Microsoft.EntityFrameworkCore.Storage;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service.AdminService;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services
{
    public class AdminService : IAdminService
    {
        #region Private Fields

        private readonly IRepository<int, Student> _studentRepository;
        private readonly IRepository<int, Faculty> _facultyRepository;
        private readonly ILogger<AdminService> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminService"/> class.
        /// </summary>
        /// <param name="studentRepository">The repository for student data.</param>
        /// <param name="facultyRepository">The repository for faculty data.</param>
        public AdminService(IRepository<int, Student> studentRepository, 
            IRepository<int, Faculty> facultyRepository,
            ILogger<AdminService> logger)
        {
            _studentRepository = studentRepository;
            _facultyRepository = facultyRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        #region Summary
        /// <summary>
        /// Activates a faculty member by email.
        /// </summary>
        /// <param name="facultyEmail">The email of the faculty member to activate.</param>
        /// <returns>A message indicating the activation status.</returns>
        /// <exception cref="NoSuchFacultyExistException">Thrown when no faculty member with the provided email exists.</exception>
        /// <exception cref="FacultyAlreadyActivatedException">Thrown when the faculty member is already activated.</exception>
        #endregion
        public async Task<string> ActivateFaculty(string facultyEmail)
        {
            var faculty = await GetFacultyByEmail(facultyEmail);
            if (faculty == null)
            {
                _logger.LogError($"Faculty with email {facultyEmail} doesn't exist in the database");
                throw new NoSuchFacultyExistException($"Faculty with email {facultyEmail} doesn't exist in the database");
            }

            if (faculty.Status == ActivationStatus.Active)
            {
                _logger.LogError($"Faculty with email {facultyEmail} is already activated");
                throw new FacultyAlreadyActivatedException($"Faculty with eamil {facultyEmail} is already activated");
            }

            faculty.Status = ActivationStatus.Active;
            await _facultyRepository.Update(faculty);
            return "Faculty account activated successfully";
        }

        #region Summary
        /// <summary>
        /// Activates a student by email.
        /// </summary>
        /// <param name="studentEmail">The email of the student to activate.</param>
        /// <returns>A message indicating the activation status.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when no student with the provided email exists.</exception>
        /// <exception cref="StudentAlreadyActivatedException">Thrown when the student is already activated.</exception>
        #endregion
        public async Task<string> ActivateStudent(string studentEmail)
        {
            var student = await GetStudentByEmail(studentEmail);
            if (student == null)
            {
                _logger.LogError($"Student with email {studentEmail} doesn't exist in the database");
                throw new NoSuchStudentExistException($"Student with email {studentEmail} doesn't exist in the database");
            }

            if (student.Status == ActivationStatus.Active)
            {
                _logger.LogError($"Student with email {studentEmail} is already activated");
                throw new StudentAlreadyActivatedException($"Student with email {studentEmail} is already activated");
            }

            student.Status = ActivationStatus.Active;
            await _studentRepository.Update(student);
            return "Student account activated successfully";
        }

        #endregion

        #region Private Methods

        #region Summary
        /// <summary>
        /// Retrieves a student by email.
        /// </summary>
        /// <param name="email">The email of the student to retrieve.</param>
        /// <returns>The student object if found, otherwise null.</returns>
        #endregion
        private async Task<Student> GetStudentByEmail(string email)
        {
            return (await _studentRepository.GetAll()).FirstOrDefault(s => s.Email == email);
        }

        #region Summary
        /// <summary>
        /// Retrieves a faculty member by email.
        /// </summary>
        /// <param name="email">The email of the faculty member to retrieve.</param>
        /// <returns>The faculty member object if found, otherwise null.</returns>
        #endregion
        private async Task<Faculty> GetFacultyByEmail(string email)
        {
            return (await _facultyRepository.GetAll()).FirstOrDefault(f => f.Email == email);
        }

        #endregion
    }

}
