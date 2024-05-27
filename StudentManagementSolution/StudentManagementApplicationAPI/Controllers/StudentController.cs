using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.CommonExceptions;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/v1/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        #region Private Fields

        private readonly IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> _authRegisterService;
        private readonly IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> _authLoginService;
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        #endregion

        #region Constructor

        public StudentController(IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> authRegisterService,
            IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> authLoginService,
            IStudentService studentService, ILogger<StudentController> logger)
        {
            _authLoginService = authLoginService;
            _authRegisterService = authRegisterService;
            _studentService = studentService;
            _logger = logger;
        }

        #endregion

        #region End Points

        /// <summary>
        /// Authenticates a student.
        /// </summary>
        /// <param name="studentLoginDTO">The login data transfer object containing student credentials.</param>
        /// <returns>An ActionResult containing the authentication result.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(StudentLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StudentLoginReturnDTO>> Login(StudentLoginDTO studentLoginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authLoginService.Login(studentLoginDTO);
                    return Ok(result);
                }
                catch (UserNotActivatedException ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel(403, ex.Message));
                }
                catch (UnauthorizedUserException ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status401Unauthorized, new ErrorModel(401, ex.Message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred: {ex.Message}"));
                }
            }
            return BadRequest("Validation Error! Provide Valid details...");
        }

        /// <summary>
        /// Registers a student.
        /// </summary>
        /// <param name="studentRegisterDTO">The registration data transfer object containing student details.</param>
        /// <returns>An ActionResult containing the registration result.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(StudentRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentRegisterReturnDTO>> Register(StudentRegisterDTO studentRegisterDTO)
        {
            try
            {
                var result = await _authRegisterService.Register(studentRegisterDTO);
                return Ok(result);
            }
            catch (CannotAddStudentToAdminDepartmentException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, ex.Message));
            }
            catch (NoSuchDepartmentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (DuplicateEmailException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddStudentException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }


        #region Summary
        /// <summary>
        /// Updates a student's details.
        /// </summary>
        /// <param name="studentDTO">The student data transfer object containing updated student details.</param>
        /// <returns>An ActionResult containing the updated student details.</returns>
        #endregion
        [HttpPut("update")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> UpdateStudent(StudentDTO studentDTO, string email)
        {
            try
            {
                var result = await _studentService.UpdateStudent(studentDTO, email);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (CannotAddStudentToAdminDepartmentException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, ex.Message));
            }
            catch (NoSuchDepartmentExistException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateStudentException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #region Summary
        /// <summary>
        /// Deletes a student by email.
        /// </summary>
        /// <param name="email">The email of the student to be deleted.</param>
        /// <returns>An ActionResult containing the deleted student details.</returns>
        #endregion
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> DeleteStudent(string email)
        {
            try
            {
                var result = await _studentService.DeleteStudent(email);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteStudentException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #region Summary
        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <returns>An ActionResult containing a list of all students.</returns>
        #endregion
        [Authorize(Roles = "Admin,Assistant_Proffesors,Associate_Proffesors,Proffesors,Head_Of_Department")]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            try
            {
                var result = await _studentService.GetAllStudents();
                return Ok(result);
            }
            catch(NoStudentsExistsException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #region Summary
        /// <summary>
        /// Retrieves a student by email.
        /// </summary>
        /// <param name="email">The email of the student to be retrieved.</param>
        /// <returns>An ActionResult containing the student details.</returns>
        #endregion
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByEmail(string email)
        {
            try
            {
                var result = await _studentService.GetStudentByEmail(email);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #region Summary
        /// <summary>
        /// Retrieves a student by their roll number.
        /// </summary>
        /// <param name="studentRollNo">The roll number of the student to be retrieved.</param>
        /// <returns>An ActionResult containing the student details.</returns>
        #endregion
        [HttpGet("id/{studentRollNo}")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int studentRollNo)
        {
            try
            {
                var result = await _studentService.GetStudentById(studentRollNo);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }


        #region Summary
        /// <summary>
        /// Retrieves the students by their names.
        /// </summary>
        /// <param name="name">The name of the student to be retrieved.</param>
        /// <returns>An ActionResult containing the student details.</returns>
        #endregion
        [HttpGet("name")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentsByName(string name)
        {
            try
            {
                var result = await _studentService.GetStudentByName(name);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #region Summary
        /// <summary>
        /// Retrieves students by their department.
        /// </summary>
        /// <param name="departmentId">The ID of the department to retrieve students from.</param>
        /// <returns>An ActionResult containing a list of students in the specified department.</returns>
        #endregion
        [Authorize(Roles = "Admin,Assistant_Proffesors,Associate_Proffesors,Proffesors,Head_Of_Department")]
        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsByDepartment(int departmentId)
        {
            try
            {
                var result = await _studentService.GetStudentsByDepartment(departmentId);
                return Ok(result);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoStudentsExistsException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #endregion
    }

}
