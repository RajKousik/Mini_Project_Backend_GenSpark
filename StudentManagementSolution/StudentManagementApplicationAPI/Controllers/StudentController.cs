using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        #region Private Fields

        private readonly IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> _authRegisterService;
        private readonly IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> _authLoginService;

        #endregion

        #region Constructor

        public StudentController(IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO> authRegisterService,
            IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO> authLoginService)
        {
            _authLoginService = authLoginService;
            _authRegisterService = authRegisterService;
        }

        #endregion

        #region Public Methods

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
                    return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel(403, ex.Message));
                }
                catch (UnauthorizedUserException ex)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new ErrorModel(401, ex.Message));
                }
                catch (Exception ex)
                {
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
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, ex.Message));
            }
            catch (NoSuchDepartmentExistException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (DuplicateEmailException ex)
            {
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddStudentException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        #endregion
    }

}
