using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.ErrorModels;

namespace StudentManagementApplicationAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        #region Private Fields

        private readonly IAdminService _adminService;

        #endregion

        #region Constructor

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #endregion

        #region End Points

        /// <summary>
        /// Activates a faculty member by email.
        /// </summary>
        /// <param name="email">The email of the faculty member to activate.</param>
        /// <returns>An IActionResult containing the result of the activation operation.</returns>
        [HttpPut("activate/faculty")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateFaculty(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ErrorModel(400, "Faculty email is required."));
            }

            try
            {
                var result = await _adminService.ActivateFaculty(email);
                return Ok(result);
            }
            catch (NoSuchFacultyExistException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"Internal server error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Activates a student by email.
        /// </summary>
        /// <param name="email">The email of the student to activate.</param>
        /// <returns>An IActionResult containing the result of the activation operation.</returns>
        [HttpPut("activate/student")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateStudent([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ErrorModel(400, "Student email is required."));
            }

            try
            {
                var result = await _adminService.ActivateStudent(email);
                return Ok(result);
            }
            catch (NoSuchStudentExistException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"Internal server error: {ex.Message}"));
            }
        }

        #endregion
    }

}
