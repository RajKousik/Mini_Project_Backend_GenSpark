using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.DTOs.CourseDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{

    [Route("api/v1/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        #region Private Fields
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the CourseController.
        /// </summary>
        /// <param name="courseService">The injected course service.</param>
        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }
        #endregion

        #region EndPoints
        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="courseDTO">The data transfer object containing course details.</param>
        /// <returns>An ActionResult containing the added course details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        [ProducesResponseType(typeof(CourseReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseReturnDTO>> AddCourse(CourseDTO courseDTO)
        {
            try
            {
                var result = await _courseService.AddCourse(courseDTO);
                return CreatedAtAction(nameof(GetCourseById), new { departmentId = result.CourseId}, result);
            }
            catch (CourseAlreadyExistsException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToAddCourseException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all courses.
        /// </summary>
        /// <returns>An ActionResult containing a list of all courses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseReturnDTO>>> GetAllCourses()
        {
            try
            {
                var result = await _courseService.GetAllCourses();
                return Ok(result);
            }
            catch (NoCoursesExistsException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course to be retrieved.</param>
        /// <returns>An ActionResult containing the course details.</returns>
        [HttpGet("{courseId}")]
        [ProducesResponseType(typeof(CourseReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseReturnDTO>> GetCourseById(int courseId)
        {
            try
            {
                var result = await _courseService.GetCourseById(courseId);
                return Ok(result);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a course by its name.
        /// </summary>
        /// <param name="name">The name of the course to be retrieved.</param>
        /// <returns>An ActionResult containing the course details.</returns>
        [HttpGet]
        [Route("name")]
        [ProducesResponseType(typeof(IEnumerable<CourseReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseReturnDTO>>> GetCourseByName(string name)
        {
            try
            {
                var result = await _courseService.GetCourseByName(name);
                return Ok(result);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves courses by the faculty ID.
        /// </summary>
        /// <param name="facultyId">The ID of the faculty to retrieve courses from.</param>
        /// <returns>An ActionResult containing a list of courses in the specified faculty.</returns>
        [HttpGet]
        [Route("facultyId")]
        [ProducesResponseType(typeof(IEnumerable<CourseReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseReturnDTO>>> GetCourseByFaculty(int facultyId)
        {
            try
            {
                var result = await _courseService.GetCourseByFaculty(facultyId);
                return Ok(result);
            }
            catch (NoCoursesExistForFacultyException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }


        /// <summary>
        /// Updates a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course to be updated.</param>
        /// <param name="courseDTO">The data transfer object containing updated course details.</param>
        /// <returns>An ActionResult containing the updated course details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{courseId}")]
        [ProducesResponseType(typeof(CourseReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseReturnDTO>> UpdateCourse(int courseId, CourseDTO courseDTO)
        {
            try
            {
                var result = await _courseService.UpdateCourse(courseId, courseDTO);
                return Ok(result);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (CourseAlreadyExistsException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToUpdateCourseException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }

        #endregion
    }

}
