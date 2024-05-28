using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.UnAuthorizationExceptions;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Interfaces.Service.AuthService;
using StudentManagementApplicationAPI.Interfaces.Service.TokenService;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Models.ErrorModels;
using StudentManagementApplicationAPI.Services;
using WatchDog;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/v1/faculty")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        #region Private Fields

        private readonly IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO> _authRegisterService;
        private readonly IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO> _authLoginService;
        private readonly IFacultyService _facultyService;
        private readonly ILogger<FacultyController> _logger;
        private readonly ITokenService _tokenService;
        //private readonly TokenManagerMiddleware _tokenManagerMiddleware;

        #endregion

        #region Constructor

        public FacultyController(IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO> authRegisterService,
                                 IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO> authLoginService,
                                 IFacultyService facultyService, ILogger<FacultyController> logger,
                                 ITokenService tokenService
                                 //, TokenManagerMiddleware tokenManagerMiddleware
            )
        {
            _authLoginService = authLoginService;
            _authRegisterService = authRegisterService;
            _facultyService = facultyService;
            _logger = logger;
            _tokenService = tokenService;
            //_tokenManagerMiddleware = tokenManagerMiddleware;
        }

        #endregion

        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    // Perform logout logic...
        //    var claim = User.Claims.First();
        //    _tokenService.AddLoggedOutClaim(claim, _tokenManagerMiddleware);
        //    return Ok();
        //}

        #region End Points

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
                    WatchLogger.Log(ex.Message);
                    _logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel(403, ex.Message));
                }
                catch (UnauthorizedUserException ex)
                {
                    WatchLogger.Log(ex.Message);
                    _logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status401Unauthorized, new ErrorModel(401, ex.Message));
                }
                catch (Exception ex)
                {
                    WatchLogger.Log(ex.Message);
                    _logger.LogError(ex.Message);
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
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (DuplicateEmailException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddFacultyException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
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
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (DuplicateEmailException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddFacultyException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
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
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (DuplicateEmailException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddFacultyException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
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
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (DuplicateEmailException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddFacultyException ex) // Replace with specific Faculty registration exception
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred : {ex.Message}"));
            }
        }


        /// <summary>
        /// Updates the details of a faculty member.
        /// </summary>
        /// <param name="dto">The data transfer object containing updated faculty details.</param>
        /// <param name="email">The email of the faculty to update.</param>
        /// <returns>The updated faculty data transfer object.</returns>
        [Authorize(Roles = "Admin,Assistant_Proffesors,Associate_Proffesors,Proffesors,Head_Of_Department")]
        [HttpPut("update/{email}")]
        [ProducesResponseType(typeof(FacultyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyDTO>> UpdateFaculty(FacultyDTO dto, string email)
        {
            try
            {
                var updatedFaculty = await _facultyService.UpdateFaculty(dto, email);
                return Ok(updatedFaculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateFacultyException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a faculty member.
        /// </summary>
        /// <param name="email">The email of the faculty to delete.</param>
        /// <returns>The deleted faculty data transfer object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{email}")]
        [ProducesResponseType(typeof(FacultyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyDTO>> DeleteFaculty(string email)
        {
            try
            {
                var deletedFaculty = await _facultyService.DeleteFaculty(email);
                return Ok(deletedFaculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteFacultyException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a faculty member by their ID.
        /// </summary>
        /// <param name="facultyId">The ID of the faculty.</param>
        /// <returns>The faculty data transfer object.</returns>
        /// 
        [HttpGet("{facultyId}")]
        [ProducesResponseType(typeof(FacultyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyDTO>> GetFacultyById(int facultyId)
        {
            try
            {
                var faculty = await _facultyService.GetFacultyById(facultyId);
                return Ok(faculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a faculty member by their email.
        /// </summary>
        /// <param name="email">The email of the faculty.</param>
        /// <returns>The faculty data transfer object.</returns>
        [HttpGet("email")]
        [ProducesResponseType(typeof(FacultyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyDTO>> GetFacultyByEmail(string email)
        {
            try
            {
                var faculty = await _facultyService.GetFacultyByEmail(email);
                return Ok(faculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves faculty members by their name.
        /// </summary>
        /// <param name="name">The name of the faculty.</param>
        /// <returns>The list of faculty data transfer objects.</returns>
        [HttpGet("name")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetFacultyByName(string name)
        {
            try
            {
                var faculties = await _facultyService.GetFacultyByName(name);
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all faculty members.
        /// </summary>
        /// <returns>The list of all faculty data transfer objects.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetAll()
        {
            try
            {
                var faculties = await _facultyService.GetAll();
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all professors.
        /// </summary>
        /// <returns>The list of professor data transfer objects.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("professors")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetProfessors()
        {
            try
            {
                var faculties = await _facultyService.GetProfessors();
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all associate professors.
        /// </summary>
        /// <returns>The list of associate professor data transfer objects.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("associate-professors")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetAssociateProfessors()
        {
            try
            {
                var faculties = await _facultyService.GetAssociateProfessors();
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all assistant professors.
        /// </summary>
        /// <returns>The list of assistant professor data transfer objects.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("assistant-professors")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetAssistantProfessors()
        {
            try
            {
                var faculties = await _facultyService.GetAssistantProfessors();
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all heads of department.
        /// </summary>
        /// <returns>The list of head of department data transfer objects.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("heads-of-department")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetHeadOfDepartment()
        {
            try
            {
                var faculties = await _facultyService.GetHeadOfDepartment();
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves faculty members by department ID.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>The list of faculty data transfer objects in the specified department.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(typeof(IEnumerable<FacultyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetFacultiesByDepartment(int departmentId)
        {
            try
            {
                var faculties = await _facultyService.GetFacultiesByDepartment(departmentId);
                return Ok(faculties);
            }
            catch (NoFacultiesExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }


        /// <summary>
        /// Changes the department of a faculty member.
        /// </summary>
        /// <param name="facultyId">The ID of the faculty member.</param>
        /// <param name="deptId">The ID of the new department.</param>
        /// <returns>An ActionResult containing the updated faculty DTO.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("change-department")]
        [ProducesResponseType(typeof(FacultyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FacultyDTO>> ChangeDepartment(int facultyId, int deptId)
        {
            try
            {
                var updatedFaculty = await _facultyService.ChangeDepartment(facultyId, deptId);
                return Ok(updatedFaculty);
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchDepartmentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateDepartmentException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, $"An unexpected error occurred: {ex.Message}"));
            }
        }


        #endregion

    }
}
