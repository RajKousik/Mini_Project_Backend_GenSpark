using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/faculty")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        #region Private Fields

        private readonly IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO> _authRegisterService;
        private readonly IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO> _authLoginService;

        #endregion

        #region Constructor

        public FacultyController(IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO> authRegisterService,
                                 IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO> authLoginService)
        {
            _authLoginService = authLoginService;
            _authRegisterService = authRegisterService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Authenticates a faculty member.
        /// </summary>
        /// <param name="facultyLoginDTO">The login data transfer object containing faculty credentials.</param>
        /// <returns>An ActionResult containing the authentication result.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(FacultyLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<FacultyLoginReturnDTO>> Login(FacultyLoginDTO facultyLoginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authLoginService.Login(facultyLoginDTO);
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
        /// Registers an assistant professor.
        /// </summary>
        /// <param name="facultyRegisterDTO">The registration data transfer object containing faculty details.</param>
        /// <returns>An ActionResult containing the registration result.</returns>
        [HttpPost("assistant-prof/register")]
        [ProducesResponseType(typeof(FacultyRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyRegisterReturnDTO>> AsstProfRegister(FacultyRegisterDTO facultyRegisterDTO)
        {
            try
            {
                var result = await _authRegisterService.Register(facultyRegisterDTO, RoleType.Assistant_Proffesors);
                return Ok(result);
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
            catch (UnableToAddFacultyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        /// <summary>
        /// Registers an associate professor.
        /// </summary>
        /// <param name="facultyRegisterDTO">The registration data transfer object containing faculty details.</param>
        /// <returns>An ActionResult containing the registration result.</returns>
        [HttpPost("associate-prof/register")]
        [ProducesResponseType(typeof(FacultyRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyRegisterReturnDTO>> AssocProfRegister(FacultyRegisterDTO facultyRegisterDTO)
        {
            try
            {
                var result = await _authRegisterService.Register(facultyRegisterDTO, RoleType.Associate_Proffesors);
                return Ok(result);
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
            catch (UnableToAddFacultyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        /// <summary>
        /// Registers a department head.
        /// </summary>
        /// <param name="facultyRegisterDTO">The registration data transfer object containing faculty details.</param>
        /// <returns>An ActionResult containing the registration result.</returns>
        [HttpPost("head-of-dept/register")]
        [ProducesResponseType(typeof(FacultyRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyRegisterReturnDTO>> DepartmentHeadRegister(FacultyRegisterDTO facultyRegisterDTO)
        {
            try
            {
                var result = await _authRegisterService.Register(facultyRegisterDTO, RoleType.Head_Of_Department);
                return Ok(result);
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
            catch (UnableToAddFacultyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }

        /// <summary>
        /// Registers an administrator.
        /// </summary>
        /// <param name="facultyRegisterDTO">The registration data transfer object containing faculty details.</param>
        /// <returns>An ActionResult containing the registration result.</returns>
        [HttpPost("admin/register")]
        [ProducesResponseType(typeof(FacultyRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyRegisterReturnDTO>> AdminRegister(FacultyRegisterDTO facultyRegisterDTO)
        {
            try
            {
                var result = await _authRegisterService.Register(facultyRegisterDTO, RoleType.Admin);
                return Ok(result);
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
            catch (UnableToAddFacultyException ex) // Replace with specific Faculty registration exception
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
