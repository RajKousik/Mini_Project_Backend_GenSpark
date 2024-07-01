using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentAttendanceDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services.Student_Service
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        #region Private Fields
        private readonly IRepository<int, Student> _studentRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly IRepository<int, StudentAttendance> _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<int, CourseRegistration> _courseRegistrationRepository;
        private readonly ILogger<StudentAttendanceService> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the StudentAttendanceService.
        /// </summary>
        /// <param name="studentRepository">The injected student repository.</param>
        /// <param name="courseRepository">The injected course repository.</param>
        /// <param name="attendanceRepository">The injected attendance repository.</param>
        public StudentAttendanceService(IRepository<int, Student> studentRepository,
            IRepository<int, Course> courseRepository,
            IRepository<int, CourseRegistration> courseRegistrationRepository,
            IMapper mapper,
            IRepository<int, StudentAttendance> attendanceRepository,
            ILogger<StudentAttendanceService> logger)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _courseRegistrationRepository = courseRegistrationRepository;
            _logger = logger;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Marks the attendance for a student.
        /// </summary>
        /// <param name="attendanceDTO">The data transfer object containing attendance details.</param>
        /// <returns>A Task representing the asynchronous operation, with a result containing the marked attendance details.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student does not exist.</exception>
        /// <exception cref="NoSuchCourseExistException">Thrown when the course does not exist.</exception>
        /// <exception cref="StudentNotOptedForCourseException">Thrown when the student has not opted for the course.</exception>
        /// <exception cref="AttendanceRecordAlreadyExistsException">Thrown when an attendance record for the student, course, and date already exists.</exception>
        /// <exception cref="InvalidAttendanceDateException">Thrown when the attendance date is in the future.</exception>
        public async Task<AttendanceReturnDTO> MarkAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                await ValidateAttendanceDTO(attendanceDTO);

                var status = attendanceDTO.AttendanceStatus.ToLower(); // Convert input string to lowercase
                if (!Enum.TryParse<AttendanceStatus>(status, true, out var attendanceStatus)) // Parse case-insensitively
                {
                    throw new InvalidAttendanceStatusException("Invalid attendance status. Attendance Status should be either 'Present', 'Absent' or 'Od'");
                }

                var attendance = new StudentAttendance
                {
                    StudentRollNo = attendanceDTO.StudentRollNo,
                    CourseId = attendanceDTO.CourseId,
                    Date = attendanceDTO.Date.ToDateTime(new TimeOnly()),
                    AttendanceStatus = attendanceStatus
                };

                var addedAttendance = await _attendanceRepository.Add(attendance);
                return _mapper.Map<AttendanceReturnDTO>(addedAttendance);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException($"Unable to mark attendance: {ex.Message}");
            }
            catch (InvalidAttendanceStatusException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidAttendanceStatusException($"Unable to mark attendance: {ex.Message}");
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException($"Unable to mark attendance: {ex.Message}");
            }
            catch (StudentNotOptedForCourseException ex)
            {
                _logger.LogError(ex.Message);
                throw new StudentNotOptedForCourseException($"Unable to mark attendance: {ex.Message}");
            }
            catch (AttendanceRecordAlreadyExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new AttendanceRecordAlreadyExistsException($"Unable to mark attendance: {ex.Message}");
            }
            catch (InvalidAttendanceDateException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidAttendanceDateException($"Unable to mark attendance: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToAddStudentAttendanceException($"Unable to mark attendance: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves an attendance record by its ID.
        /// </summary>
        /// <param name="attendanceId">The ID of the attendance record to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of the attendance data transfer object.</returns>
        /// <exception cref="Exception">Throws a general exception if an error occurs.</exception>
        public async Task<AttendanceReturnDTO> GetAttendance(int attendanceId)
        {
            try
            {
                var attendance = await _attendanceRepository.GetById(attendanceId);
                if (attendance == null)
                {
                    throw new NoSuchStudentAttendanceExistException($"Attendance record with ID {attendanceId} does not exist.");
                }
                return _mapper.Map<AttendanceReturnDTO>(attendance);
            }
            catch (NoSuchStudentAttendanceExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentAttendanceExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An error occurred while retrieving the attendance record: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves all attendance records.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of attendance data transfer objects.</returns>
        /// <exception cref="Exception">Throws a general exception if an error occurs.</exception>
        public async Task<IEnumerable<AttendanceReturnDTO>> GetAllAttendanceRecords()
        {
            try
            {
                var attendanceRecords = (await _attendanceRepository.GetAll()).ToList();
                if (attendanceRecords.Count == 0)
                {
                    throw new NoStudentAttendancesExistsException();
                }
                return _mapper.Map<IEnumerable<AttendanceReturnDTO>>(attendanceRecords);
            }
            catch (NoStudentAttendancesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoStudentAttendancesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An error occurred while retrieving all attendance records: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves attendance records for a specific student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of attendance data transfer objects for the student.</returns>
        /// <exception cref="Exception">Throws a general exception if an error occurs.</exception>
        public async Task<IEnumerable<AttendanceReturnDTO>> GetStudentAttendanceRecords(int studentId)
        {
            try
            {
                var student = await _studentRepository.GetById(studentId);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with ID {studentId} does not exist.");
                }

                var attendanceRecords = await GetAttendanceRecordsByStudentId(studentId);
                if (!attendanceRecords.Any())
                {
                    throw new NoSuchStudentAttendanceExistException($"No attendance records found for student with ID {studentId}.");
                }

                return _mapper.Map<IEnumerable<AttendanceReturnDTO>>(attendanceRecords);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException($"Unable to retrieve attendance records: {ex.Message}");
            }
            catch (NoSuchStudentAttendanceExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentAttendanceExistException($"Unable to retrieve attendance records: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An error occurred while retrieving attendance records for student with ID {studentId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves attendance records for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of attendance data transfer objects for the course.</returns>
        /// <exception cref="Exception">Throws a general exception if an error occurs.</exception>
        public async Task<IEnumerable<AttendanceReturnDTO>> GetCourseAttendanceRecords(int courseId)
        {
            try
            {
                var course = await _courseRepository.GetById(courseId);
                if (course == null)
                {
                    throw new NoSuchCourseExistException($"Course with ID {courseId} does not exist.");
                }

                var attendanceRecords = await GetAttendanceRecordsByCourseId(courseId);
                if (!attendanceRecords.Any())
                {
                    throw new NoSuchStudentAttendanceExistException($"No attendance records found for course with ID {courseId}.");
                }

                return _mapper.Map<IEnumerable<AttendanceReturnDTO>>(attendanceRecords);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException($"Unable to retrieve attendance records: {ex.Message}");
            }
            catch (NoSuchStudentAttendanceExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentAttendanceExistException($"Unable to retrieve attendance records: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An error occurred while retrieving attendance records for course with ID {courseId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Updates an existing attendance record.
        /// </summary>
        /// <param name="attendanceId">The ID of the attendance record to update.</param>
        /// <param name="attendanceStatus">The updated attendance status.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of the updated attendance data transfer object.</returns>
        /// <exception cref="NoSuchAttendanceRecordExistException">Thrown when no attendance record with the specified ID exists.</exception>
        /// <exception cref="InvalidAttendanceStatusException">Thrown when the provided attendance status is invalid.</exception>
        /// <exception cref="InvalidAttendanceUpdateException">Thrown when trying to update attendance to the same status.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the update process.</exception>
        public async Task<AttendanceReturnDTO> UpdateAttendance(int attendanceId, string attendanceStatus)
        {
            try
            {


                var existingAttendance = await _attendanceRepository.GetById(attendanceId);
                if (existingAttendance == null)
                {
                    throw new NoSuchStudentAttendanceExistException($"Attendance record with ID {attendanceId} does not exist.");
                }

                if (!Enum.TryParse<AttendanceStatus>(attendanceStatus, true, out var parsedStatus))
                {
                    throw new InvalidAttendanceStatusException("Invalid attendance status. Attendance Status should be either 'Present', 'Absent' or 'Od'");
                }

                if (existingAttendance.AttendanceStatus == parsedStatus)
                {
                    throw new InvalidAttendanceUpdateException($"Attendance status is already {parsedStatus}.");
                }


                existingAttendance.AttendanceStatus = parsedStatus;
                var updatedAttendance = await _attendanceRepository.Update(existingAttendance);
                return _mapper.Map<AttendanceReturnDTO>(updatedAttendance);
            }
            catch (NoSuchStudentAttendanceExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentAttendanceExistException($"Unable to update attendance: {ex.Message}");
            }
            catch (InvalidAttendanceStatusException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidAttendanceStatusException($"Unable to update attendance: {ex.Message}");
            }
            catch (InvalidAttendanceUpdateException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidAttendanceUpdateException($"Unable to update attendance: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An error occurred while updating attendance record with ID {attendanceId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Deletes an existing attendance record.
        /// </summary>
        /// <param name="attendanceId">The ID of the attendance record to delete.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of the deleted attendance data transfer object.</returns>
        /// <exception cref="NoSuchAttendanceRecordExistException">Thrown when no attendance record with the specified ID exists.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the delete process.</exception>
        public async Task<AttendanceReturnDTO> DeleteAttendance(int attendanceId)
        {
            try
            {
                var existingAttendance = await _attendanceRepository.GetById(attendanceId);
                if (existingAttendance == null)
                {
                    throw new NoSuchStudentAttendanceExistException($"Attendance record with ID {attendanceId} does not exist.");
                }

                var deletedAttendance = await _attendanceRepository.Delete(attendanceId);
                return _mapper.Map<AttendanceReturnDTO>(deletedAttendance);
            }
            catch (NoSuchStudentAttendanceExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentAttendanceExistException($"Unable to delete attendance: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"An error occurred while deleting attendance record with ID {attendanceId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves the attendance percentage for a student in each course.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a collection of AttendancePercentageDTO objects containing course ID and attendance percentage.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when no student with the specified ID exists.</exception>
        /// <exception cref="NoAttendanceRecordsExistException">Thrown when no attendance records exist for the student.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the retrieval process.</exception>
        public async Task<IEnumerable<AttendancePercentageDTO>> GetStudentAttendancePercentage(int studentId)
        {
            try
            {
                var student = await _studentRepository.GetById(studentId);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with ID {studentId} does not exist.");
                }

                var result = await GetAttendanceRecordsByStudentId(studentId);
                var attendanceRecords = _mapper.Map<IEnumerable<AttendanceReturnDTO>>(result);
                //var attendanceRecords = await GetAttendanceRecordsByStudentId(studentId);
                if (attendanceRecords.Count() == 0)
                {
                    throw new NoStudentAttendancesExistsException($"No attendance records found for student with ID {studentId}.");
                }

                var attendancePercentageDTOs = new List<AttendancePercentageDTO>();
                var courseIds = attendanceRecords.Select(ar => ar.CourseId).Distinct();

                foreach (var courseId in courseIds)
                {
                    var attendanceRecordsForCourse = attendanceRecords.Where(ar => ar.CourseId == courseId);
                    double totalAttendance = attendanceRecordsForCourse.Count();
                    double presentCount = attendanceRecordsForCourse.Count(ar => ar.AttendanceStatus == AttendanceStatus.Present.ToString()
                                                                            || ar.AttendanceStatus == AttendanceStatus.Od.ToString());
                    double attendancePercentage = presentCount / totalAttendance * 100;

                    attendancePercentageDTOs.Add(new AttendancePercentageDTO
                    {
                        CourseId = courseId,
                        AttendancePercentage = attendancePercentage
                    });
                }

                return attendancePercentageDTOs;
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (NoStudentAttendancesExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoStudentAttendancesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Validates the AttendanceDTO object by checking various conditions such as the existence of the student, course,
        /// whether the student has opted for the course, whether an attendance record for the student, course, and date
        /// already exists, and whether the attendance date is not in the future.
        /// </summary>
        /// <param name="attendanceDTO">The AttendanceDTO object to be validated.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student does not exist.</exception>
        /// <exception cref="NoSuchCourseExistException">Thrown when the course does not exist.</exception>
        /// <exception cref="StudentNotOptedForCourseException">Thrown when the student has not opted for the course.</exception>
        /// <exception cref="AttendanceRecordAlreadyExistsException">Thrown when an attendance record for the student, course, and date already exists.</exception>
        /// <exception cref="InvalidAttendanceDateException">Thrown when the attendance date is in the future.</exception>
        private async Task ValidateAttendanceDTO(AttendanceDTO attendanceDTO)
        {

            // Check if the student exists
            var student = await _studentRepository.GetById(attendanceDTO.StudentRollNo);
            if (student == null)
            {
                throw new NoSuchStudentExistException($"Student with roll number {attendanceDTO.StudentRollNo} does not exist.");
            }

            // Check if the course exists
            var course = await _courseRepository.GetById(attendanceDTO.CourseId);
            if (course == null)
            {
                throw new NoSuchCourseExistException($"Course with ID {attendanceDTO.CourseId} does not exist.");
            }

            // Check if the student has opted for the course
            bool isOptedForCourse = await IsStudentOptedForCourse(attendanceDTO.StudentRollNo, attendanceDTO.CourseId);
            if (!isOptedForCourse)
            {
                throw new StudentNotOptedForCourseException($"Student with roll number {attendanceDTO.StudentRollNo} has not opted for the course with ID {attendanceDTO.CourseId}.");
            }

            // Check if an attendance record for the student, course, and date already exists
            var existingAttendance = await AttendanceRecordExists(attendanceDTO.StudentRollNo, attendanceDTO.CourseId, attendanceDTO.Date);
            if (existingAttendance)
            {
                throw new AttendanceRecordAlreadyExistsException($"Attendance record for student with roll number {attendanceDTO.StudentRollNo}, course with ID {attendanceDTO.CourseId}, and date {attendanceDTO.Date} already exists.");
            }

            // Check if the attendance date is in the future
            if (attendanceDTO.Date.ToDateTime(new TimeOnly()) > DateTime.Now.Date)
            {
                throw new InvalidAttendanceDateException("Attendance date cannot be in the future.");
            }

            //check if the attendance status is valid enum type
            var status = attendanceDTO.AttendanceStatus.ToLower(); // Convert input string to lowercase
            if (!Enum.GetNames(typeof(AttendanceStatus)).Any(s => s.ToLower() == status))
            {
                throw new InvalidAttendanceStatusException("Invalid attendance status. Attendance Status should be either 'Present', 'Absent' or 'Od'");
            }
        }

        /// <summary>
        /// Checks if an attendance record exists for a given student, course, and date.
        /// </summary>
        /// <param name="studentRollNo">The roll number of the student.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="date">The date of the attendance record.</param>
        /// <returns>A Task representing the asynchronous operation, with a result indicating whether the attendance record exists.</returns>
        private async Task<bool> AttendanceRecordExists(int studentRollNo, int courseId, DateOnly date)
        {
            var attendanceRecords = await _attendanceRepository.GetAll();
            return attendanceRecords.Any(ar => ar.StudentRollNo == studentRollNo && ar.CourseId == courseId && ar.Date.Date == date.ToDateTime(new TimeOnly()).Date);
        }

        /// <summary>
        /// Checks if a student has opted for a specific course.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A Task representing the asynchronous operation, with a result indicating whether the student has opted for the course.</returns>
        private async Task<bool> IsStudentOptedForCourse(int studentId, int courseId)
        {
            var courseRegistrations = (await _courseRegistrationRepository.GetAll())
                .Where(cr => cr.StudentId == studentId && cr.CourseId == courseId && cr.ApprovalStatus == ApprovalStatus.Approved)
                .ToList();

            return courseRegistrations.Any();
        }

        /// <summary>
        /// Retrieves attendance records by student ID from the repository.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of attendance records.</returns>
        private async Task<IEnumerable<StudentAttendance>> GetAttendanceRecordsByStudentId(int studentId)
        {
            try
            {
                var isStudentExists = await _studentRepository.GetById(studentId);

                var attendanceRecords = (await _attendanceRepository.GetAll()).Where(ar => ar.StudentRollNo == studentId).ToList();
                if (attendanceRecords.Count == 0)
                {
                    throw new NoStudentAttendancesExistsException();
                }
                return attendanceRecords;
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (NoStudentAttendancesExistsException ex)
            {
                throw new NoStudentAttendancesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves attendance records by course ID from the repository.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of attendance records.</returns>
        private async Task<IEnumerable<StudentAttendance>> GetAttendanceRecordsByCourseId(int courseId)
        {
            var attendanceRecords = await _attendanceRepository.GetAll();
            return attendanceRecords.Where(ar => ar.CourseId == courseId).ToList();
        }

        #endregion
    }

}
