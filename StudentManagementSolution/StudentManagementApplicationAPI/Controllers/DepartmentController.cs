using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/v1/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Private Fields

        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        #endregion

        #region Constructor

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        #endregion

        #region End Points

        /// <summary>
        /// Adds a new department.
        /// </summary>
        /// <param name="departmentDTO">The department data transfer object.</param>
        /// <returns>The added department data transfer object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDTO>> AddDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                var addedDepartment = await _departmentService.AddDepartment(departmentDTO);
                return CreatedAtAction(nameof(GetDepartmentById), new { departmentId = addedDepartment.DeptId }, addedDepartment);
            }
            catch (DepartmentAlreadyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(409, ex.Message));
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(404, ex.Message));
            }
            catch (UnableToAddDepartmentException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a department by ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>The department data transfer object.</returns>
        [HttpGet("{departmentId}")]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDTO>> GetDepartmentById(int departmentId)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(departmentId);
                return Ok(department);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>The list of all department data transfer objects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DepartmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAllDepartments()
        {
            try
            {
                var departments = await Task.WhenAll(_departmentService.GetAllDepartments());
                return Ok(departments);
            }
            catch (NoDepartmentsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Changes the head of a department.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <param name="newHeadDepartmentId">The ID of the new head of the department.</param>
        /// <returns>The updated department data transfer object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("change-department-head")]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDTO>> ChangeDepartmentHead(int departmentId, int newHeadDepartmentId)
        {
            try
            {
                var updatedDepartment = await _departmentService.ChangeDepartmentHead(departmentId, newHeadDepartmentId);
                return Ok(updatedDepartment);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateDepartmentException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, ex.Message));
            }
            catch (UnableToUpdateFacultyException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        #endregion
    }

}
