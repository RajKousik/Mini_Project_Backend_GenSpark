using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.DTOs.StudentAttendanceDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{
    /// <summary>
    /// Controller for managing student attendance.
    /// </summary>
    [Route("api/v1/student-attendance")]
    [ApiController]

    public class StudentAttendanceController : ControllerBase
    {
        #region Private Fields

        private readonly IStudentAttendanceService _studentAttendanceService;
        private readonly ILogger<StudentAttendanceController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the StudentAttendanceController.
        /// </summary>
        /// <param name="studentAttendanceService">The injected student attendance service.</param>
        public StudentAttendanceController(IStudentAttendanceService studentAttendanceService,
                ILogger<StudentAttendanceController> logger)
        {
            _studentAttendanceService = studentAttendanceService;
            _logger = logger;
        }

        #endregion

        #region End Points

        /// <summary>
        /// Marks the attendance for a student.
        /// </summary>
        /// <param name="attendanceDTO">The data transfer object containing attendance details.</param>
        /// <returns>An ActionResult containing the marked attendance details.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceReturnDTO>> MarkAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                var result = await _studentAttendanceService.MarkAttendance(attendanceDTO);
                return CreatedAtAction(nameof(GetAttendance), new { attendanceId = result.Id }, result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (StudentNotOptedForCourseException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (AttendanceRecordAlreadyExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidAttendanceDateException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidAttendanceStatusException ex)
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
        /// Retrieves an attendance record by its ID.
        /// </summary>
        /// <param name="attendanceId">The ID of the attendance record to retrieve.</param>
        /// <returns>An ActionResult containing the attendance details.</returns>
        [HttpGet("{attendanceId}")]
        [ProducesResponseType(typeof(AttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceReturnDTO>> GetAttendance(int attendanceId)
        {
            try
            {
                var result = await _studentAttendanceService.GetAttendance(attendanceId);
                return Ok(result);
            }
            catch (NoSuchStudentAttendanceExistException ex)
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
        /// Updates an existing attendance record.
        /// </summary>
        /// <param name="attendanceId">The ID of the attendance record to update.</param>
        /// <param name="attendanceStatus">The updated attendance status.</param>
        /// <returns>An ActionResult containing the updated attendance details.</returns>
        [HttpPut("{attendanceId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceReturnDTO>> UpdateAttendance(int attendanceId, string attendanceStatus)
        {
            try
            {
                var result = await _studentAttendanceService.UpdateAttendance(attendanceId, attendanceStatus);
                return Ok(result);
            }
            catch (NoSuchStudentAttendanceExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (InvalidAttendanceStatusException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidAttendanceUpdateException ex)
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
        /// Deletes an existing attendance record.
        /// </summary>
        /// <param name="attendanceId">The ID of the attendance record to delete.</param>
        /// <returns>An ActionResult containing the deleted attendance details.</returns>
        [HttpDelete("{attendanceId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AttendanceReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceReturnDTO>> DeleteAttendance(int attendanceId)
        {
            try
            {
                var result = await _studentAttendanceService.DeleteAttendance(attendanceId);
                return Ok(result);
            }
            catch (NoSuchStudentAttendanceExistException ex)
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
        /// Retrieves all attendance records.
        /// </summary>
        /// <returns>An ActionResult containing a list of all attendance records.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendanceReturnDTO>>> GetAllAttendanceRecords()
        {
            try
            {
                var result = await _studentAttendanceService.GetAllAttendanceRecords();
                return Ok(result);
            }
            catch (NoStudentAttendancesExistsException ex)
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
        /// Retrieves attendance records for a specific student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>An ActionResult containing the list of attendance records for the student.</returns>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendanceReturnDTO>>> GetStudentAttendanceRecords(int studentId)
        {
            try
            {
                var result = await _studentAttendanceService.GetStudentAttendanceRecords(studentId);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchStudentAttendanceExistException ex)
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
        /// Retrieves attendance records for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>An ActionResult containing the list of attendance records for the course.</returns>
        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Admin,Assistant_Proffesors,Associate_Proffesors,Proffesors,Head_Of_Department")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendanceReturnDTO>>> GetCourseAttendanceRecords(int courseId)
        {
            try
            {
                var result = await _studentAttendanceService.GetCourseAttendanceRecords(courseId);
                return Ok(result);
            }
            catch (NoStudentAttendancesExistsException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchStudentAttendanceExistException ex)
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
        /// Retrieves the attendance percentage for a student in each course.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>An ActionResult containing the list of attendance percentage for each course.</returns>
        [HttpGet("attendance-percentage/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<AttendancePercentageDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendancePercentageDTO>>> GetStudentAttendancePercentage(int studentId)
        {
            try
            {
                var result = await _studentAttendanceService.GetStudentAttendancePercentage(studentId);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoStudentAttendancesExistsException ex)
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


        #endregion
    }


}
