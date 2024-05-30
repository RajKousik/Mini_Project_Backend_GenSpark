using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/v1/course-registrations")]
    [ApiController]
    public class CourseRegistrationController : ControllerBase
    {
        #region Private Fields
        private readonly ICourseRegistrationService _courseRegistrationService;
        private readonly ILogger<CourseRegistrationController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the CourseRegistrationController.
        /// </summary>
        /// <param name="courseRegistrationService">The injected course registration service.</param>
        public CourseRegistrationController(ICourseRegistrationService courseRegistrationService,
            ILogger<CourseRegistrationController> logger)
        {
            _courseRegistrationService = courseRegistrationService;
            _logger = logger;
        }
        #endregion

        #region EndPoints

        /// <summary>
        /// Adds a new course registration.
        /// </summary>
        /// <param name="courseRegistrationAddDTO">The data transfer object containing course registration details.</param>
        /// <returns>An ActionResult containing the added course registration details.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CourseRegistrationReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseRegistrationReturnDTO>> AddCourseRegistration(CourseRegistrationAddDTO courseRegistrationAddDTO)
        {
            try
            {
                var result = await _courseRegistrationService.AddCourse(courseRegistrationAddDTO);
                return CreatedAtAction(nameof(GetCourseRegistrationById), new { courseRegistrationId = result.RegistrationId }, result);
            }
            catch (InsufficientWallentBalanceException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(400, ex.Message));
            }
            catch (StudentAlreadyRegisteredForCourseException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (NoSuchCourseExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchStudentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToAddCourseRegistrationException ex)
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
        /// Retrieves all course registrations.
        /// </summary>
        /// <returns>An ActionResult containing a list of all course registrations.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseRegistrationReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseRegistrationReturnDTO>>> GetAllCourseRegistrations()
        {
            try
            {
                var result = await _courseRegistrationService.GetAllCourseRegistrations();
                return Ok(result);
            }
            catch (NoCourseRegistrationsExistsException ex)
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
        /// Retrieves a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration to be retrieved.</param>
        /// <returns>An ActionResult containing the course registration details.</returns>
        [HttpGet]
        [Route("{CourseRegistrationId}")]
        [ProducesResponseType(typeof(CourseRegistrationReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseRegistrationReturnDTO>> GetCourseRegistrationById(int courseRegistrationId)
        {
            try
            {
                var result = await _courseRegistrationService.GetCourseRegistrationById(courseRegistrationId);
                return Ok(result);
            }
            catch (NoSuchCourseRegistrationExistException ex)
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
        /// Updates a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration to be updated.</param>
        /// <param name="courseRegistrationAddDTO">The data transfer object containing updated course registration details.</param>
        /// <returns>An ActionResult containing the updated course registration details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(CourseRegistrationReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseRegistrationReturnDTO>> UpdateCourseRegistration(int courseRegistrationId, int courseId)
        {
            try
            {
                var result = await _courseRegistrationService.UpdateCourseRegistraion(courseRegistrationId, courseId);
                return Ok(result);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchCourseExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchStudentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (StudentAlreadyRegisteredForCourseException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (StudentAlreadyApprovedForCourseException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToUpdateCourseRegistrationException ex)
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
        /// Deletes a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration to be deleted.</param>
        /// <returns>An ActionResult containing the deleted course registration details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(typeof(CourseRegistrationReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseRegistrationReturnDTO>> DeleteCourseRegistration(int courseRegistrationId)
        {
            try
            {
                var result = await _courseRegistrationService.DeleteCourseRegistration(courseRegistrationId);
                return Ok(result);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnableToDeleteCourseRegistrationException ex)
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
        /// Retrieves the courses registered by a student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>An ActionResult containing a list of course registrations.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("student")]
        [ProducesResponseType(typeof(IEnumerable<CourseRegistrationReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseRegistrationReturnDTO>>> GetCoursesRegisteredByStudent(int studentId)
        {
            try
            {
                var result = await _courseRegistrationService.GetCoursesRegisteredByStudent(studentId);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoCourseRegistrationsExistsException ex)
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
        /// Retrieves the registered students for a course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>An ActionResult containing a list of course registrations.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("course")]
        [ProducesResponseType(typeof(IEnumerable<CourseRegistrationReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseRegistrationReturnDTO>>> GetRegisteredStudentsForCourse(int courseId)
        {
            try
            {
                var result = await _courseRegistrationService.GetRegisteredStudentsForCourse(courseId);
                return Ok(result);
            }
            catch (NoSuchCourseExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoCourseRegistrationsExistsException ex)
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
        /// Approves a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration to be approved.</param>
        /// <returns>An ActionResult containing the approved course registration details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("approve")]
        [ProducesResponseType(typeof(CourseRegistrationReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CourseRegistrationReturnDTO>> ApproveCourseRegistration(int courseRegistrationId)
        {
            try
            {
                var result = await _courseRegistrationService.ApproveCourseRegistrations(courseRegistrationId);
                return Ok(result);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (StudentAlreadyApprovedForCourseException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToApproveCourseRegistrationException ex)
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
        /// Approves all course registrations for a student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>An ActionResult containing a list of approved course registrations.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("approve-registrations")]
        [ProducesResponseType(typeof(IEnumerable<CourseRegistrationReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CourseRegistrationReturnDTO>>> ApproveCourseRegistrationsForStudent(int studentId)
        {
            try
            {
                var result = await _courseRegistrationService.ApproveCourseRegistrationsForStudent(studentId);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoCourseRegistrationsExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (StudentAlreadyApprovedForCourseException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToApproveCourseRegistrationException ex)
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

        #endregion
    }

}
