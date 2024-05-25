using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.DTOs.ExamDTOs;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        #region Private Fields
        private readonly IExamService _examService;
        #endregion

        #region Controller
        /// <summary>
        /// Injecting the service
        /// </summary>
        /// <param name="examService">Exam Service</param>
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }
        #endregion

        #region Endpoints



        /// <summary>
        /// Adds a new exam.
        /// </summary>
        /// <param name="examDTO">The data transfer object containing exam details.</param>
        /// <returns>The added exam data transfer object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ExamReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExamReturnDTO>> AddExam(ExamDTO examDTO)
        {
            try
            {
                var addedExam = await _examService.AddExam(examDTO);
                return CreatedAtAction(nameof(GetExamById), new { examId = addedExam.ExamId }, addedExam);
            }
            catch (NoSuchCourseExistException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(404, ex.Message));
            }
            catch (ExamAlreadyScheduledException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(409, ex.Message));
            }
            catch (InvalidTotalMarkException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(400, ex.Message));
            }
            catch (InvalidExamDateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(400, ex.Message));
            }
            catch (InvalidExamTypeException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(400, ex.Message));
            }
            catch (UnableToAddExamException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Retrieves an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The exam data transfer object.</returns>
        [HttpGet("exam-id")]
        [ProducesResponseType(typeof(ExamReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExamReturnDTO>> GetExamById(int examId)
        {
            try
            {
                var exam = await _examService.GetExamById(examId);
                return Ok(exam);
            }
            catch (NoSuchExamExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Retrieves all exams.
        /// </summary>
        /// <returns>The list of exam data transfer objects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExamReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExamReturnDTO>>> GetAllExams()
        {
            try
            {
                var exams = await _examService.GetAllExams();
                return Ok(exams);
            }
            catch (NoExamsExistsException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Retrieves upcoming exams within the specified number of days. If no value is provided for the days parameter, it defaults to 7.
        /// </summary>
        /// <param name="days">The number of days to look ahead for upcoming exams (default is 7).</param>
        /// <returns>The list of exam data transfer objects.</returns>
        [HttpGet("upcoming")]
        [ProducesResponseType(typeof(IEnumerable<ExamReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExamReturnDTO>>> GetUpcomingExams([FromQuery] int? days = null)
        {
            try
            {
                if (days == null || days <= 0)
                {
                    days = 7; // Default value
                }

                var exams = await _examService.GetUpcomingExams(days.Value);
                return Ok(exams);
            }
            catch (NoSuchExamExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }




        /// <summary>
        /// Updates an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <param name="examDTO">The data transfer object containing updated exam details.</param>
        /// <returns>The updated exam data transfer object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{examId}")]
        [ProducesResponseType(typeof(ExamReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExamReturnDTO>> UpdateExam(int examId, ExamDTO examDTO)
        {
            try
            {
                var updatedExam = await _examService.UpdateExam(examId, examDTO);
                return Ok(updatedExam);
            }
            catch (NoSuchExamExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Deletes an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The deleted exam data transfer object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{examId}")]
        [ProducesResponseType(typeof(ExamReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExamReturnDTO>> DeleteExam(int examId)
        {
            try
            {
                var deletedExam = await _examService.DeleteExam(examId);
                return Ok(deletedExam);
            }
            catch (NoSuchExamExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (CannotDeleteFinishedExamException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Retrieves exams by the specified date.
        /// </summary>
        /// <param name="date">The date of the exams.</param>
        /// <returns>The list of exam data transfer objects.</returns>
        [HttpGet("date")]
        [ProducesResponseType(typeof(IEnumerable<ExamReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExamReturnDTO>>> GetExamsByDate(DateTime date)
        {
            try
            {
                var exams = await _examService.GetExamsByDate(date);
                return Ok(exams);
            }
            catch (NoSuchCourseExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Retrieves offline exams.
        /// </summary>
        /// <returns>The list of offline exam data transfer objects.</returns>
        [HttpGet("offline")]
        [ProducesResponseType(typeof(IEnumerable<ExamReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExamReturnDTO>>> GetOfflineExams()
        {
            try
            {
                var exams = await _examService.GetOfflineExams();
                return Ok(exams);
            }
            catch (NoSuchExamExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }



        /// <summary>
        /// Retrieves online exams.
        /// </summary>
        /// <returns>The list of online exam data transfer objects.</returns>
        [HttpGet("online")]
        [ProducesResponseType(typeof(IEnumerable<ExamReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExamReturnDTO>>> GetOnlineExams()
        {
            try
            {
                var exams = await _examService.GetOnlineExams();
                return Ok(exams);
            }
            catch (NoSuchExamExistException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel(500, ex.Message));
            }
        }
    }
    #endregion
}
