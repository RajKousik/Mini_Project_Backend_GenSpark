using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.DTOs.GradeDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;
using WatchDog;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/v1/grades")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        #region Private Fields
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradeController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Injecting the required services
        /// </summary>
        /// <param name="gradeService">Grade Service</param>
        public GradeController(IGradeService gradeService, ILogger<GradeController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }
        #endregion

        #region Endpoints


        /// <summary>
        /// Adds a new grade.
        /// </summary>
        /// <param name="gradeDTO">The data transfer object containing grade details.</param>
        /// <returns>An ActionResult containing the added grade details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(GradeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GradeReturnDTO>> AddGrade(GradeDTO gradeDTO)
        {
            try
            {
                var result = await _gradeService.AddGrade(gradeDTO);
                return CreatedAtAction(nameof(GetGradeById), new { gradeId = result.Id }, result);
            }
            catch (NoSuchExamExistException ex)
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
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (StudentNotOptedForCourseException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidExamDateException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidMarksScoredException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidGradeException ex)
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
        /// Retrieves all grades.
        /// </summary>
        /// <returns>An ActionResult containing a list of all grades.</returns>
        [Authorize(Roles = "Admin,Assistant_Proffesors,Associate_Proffesors,Proffesors,Head_Of_Department")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GradeReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GradeReturnDTO>>> GetAllGrades()
        {
            try
            {
                var result = await _gradeService.GetAllGrades();
                return Ok(result);
            }
            catch (NoGradeRecordsExistsException ex)
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
        /// Retrieves a grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the grade to be retrieved.</param>
        /// <returns>An ActionResult containing the grade details.</returns>
        [HttpGet("{gradeId}")]
        [ProducesResponseType(typeof(GradeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GradeReturnDTO>> GetGradeById(int gradeId)
        {
            try
            {
                var result = await _gradeService.GetGradeById(gradeId);
                return Ok(result);
            }
            catch (NoSuchGradeRecordExistsException ex)
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
        /// Updates a grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the grade to be updated.</param>
        /// <param name="gradeUpdateDTO">The data transfer object containing updated grade details.</param>
        /// <returns>An ActionResult containing the updated grade details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{gradeId}")]
        [ProducesResponseType(typeof(GradeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GradeReturnDTO>> UpdateGrade(int gradeId, GradeUpdateDTO gradeUpdateDTO)
        {
            try
            {
                var result = await _gradeService.UpdateGrade(gradeId, gradeUpdateDTO);
                return Ok(result);
            }
            catch (NoSuchGradeRecordExistsException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoSuchFacultyExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (InvalidMarksScoredException ex)
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
        /// Deletes a grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the grade to be deleted.</param>
        /// <returns>An ActionResult indicating success or failure of the deletion operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{gradeId}")]
        [ProducesResponseType(typeof(GradeReturnDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GradeReturnDTO>> DeleteGrade(int gradeId)
        {
            try
            {
                var deletedGrade = await _gradeService.DeleteGrade(gradeId);
                return Ok(deletedGrade);
            }
            catch (NoSuchGradeRecordExistsException ex)
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
        /// Retrieves grades for a specific student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>An ActionResult containing the list of grade data transfer objects for the student.</returns>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<GradeReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GradeReturnDTO>>> GetStudentGrades(int studentId)
        {
            try
            {
                var result = await _gradeService.GetStudentGrades(studentId);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (NoGradeRecordsExistsException ex)
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
        /// Retrieves grades for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>An ActionResult containing the list of grade data transfer objects for the course.</returns>
        [Authorize(Roles = "Admin,Assistant_Proffesors,Associate_Proffesors,Proffesors,Head_Of_Department")]
        [HttpGet("course/{courseId}")]
        [ProducesResponseType(typeof(IEnumerable<GradeReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GradeReturnDTO>>> GetCourseGrades(int courseId)
        {
            try
            {
                var result = await _gradeService.GetCourseGrades(courseId);
                return Ok(result);
            }
            catch (NoGradeRecordsExistsException ex)
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
            catch (Exception ex)
            {
                WatchLogger.Log(ex.Message);
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        [HttpGet("top-students/course/{courseId}")]
        [ProducesResponseType(typeof(IEnumerable<GradeReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GradeReturnDTO>>> GetTopStudentsByCourse(int courseId)
        {
            try
            {
                var result = await _gradeService.GetTopStudentsByCourse(courseId);
                return Ok(result);
            }
            catch (NoGradeRecordsExistsException ex)
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
