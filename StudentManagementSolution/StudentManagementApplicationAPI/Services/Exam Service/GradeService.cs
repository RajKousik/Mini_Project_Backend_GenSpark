using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.GradeDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services.Exam_Service
{
    public class GradeService : IGradeService
    {
        #region CONSTANT FIELDS - For Testing 
        private readonly DateTime CURRENT_DATE_TIME = new DateTime(2025, 1, 1, 0, 0, 0);
        #endregion

        #region Private Fields

        private readonly IRepository<int, Grade> _gradeRepository;
        private readonly IRepository<int, Exam> _examRepository;
        private readonly IRepository<int, Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<int, CourseRegistration> _courseRegistrationRepository;
        private readonly IRepository<int, Faculty> _facultyRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly ILogger<GradeService> _logger;

        #endregion

        #region Constructor

        public GradeService(
        IRepository<int, Grade> gradeRepository,
        IRepository<int, Exam> examRepository,
        IRepository<int, Student> studentRepository,
        IRepository<int, CourseRegistration> courseRegistrationRepository,
        IRepository<int, Faculty> facultyRepository,
        IRepository<int, Course> courseRepository,
        IMapper mapper,
        ILogger<GradeService> logger)
        {
            _gradeRepository = gradeRepository;
            _examRepository = examRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _courseRegistrationRepository = courseRegistrationRepository;
            _facultyRepository = facultyRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new grade.
        /// </summary>
        /// <param name="gradeDTO">The data transfer object containing grade details.</param>
        /// <returns>The added grade data transfer object.</returns>
        public async Task<GradeReturnDTO> AddGrade(GradeDTO gradeDTO)
        {
            try
            {
                //check if the student exists
                await ValidateGradeDTO(gradeDTO);
                var exam = await _examRepository.GetById(gradeDTO.ExamId);
                double percentage = CalculatePercentage(gradeDTO.MarksScored, exam.TotalMark);
                var gradeType = CalculateGrade(percentage);

                // Add the grade
                var grade = _mapper.Map<Grade>(gradeDTO);

                grade.Percentage = percentage;
                grade.StudentGrade = gradeType;

                var addedGrade = await _gradeRepository.Add(grade);
                return _mapper.Map<GradeReturnDTO>(addedGrade);
            }
            catch (NoSuchExamExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchExamExistException($"Unable to add grade: {ex.Message}");
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException($"Unable to add grade: {ex.Message}");
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException($"Unable to add grade: {ex.Message}");
            }
            catch (StudentNotOptedForCourseException ex)
            {
                _logger.LogError(ex.Message);
                throw new StudentNotOptedForCourseException($"Unable to add grade: {ex.Message}");
            }
            catch (InvalidMarksScoredException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidMarksScoredException($"Unable to add grade: {ex.Message}");
            }
            catch (InvalidGradeException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidGradeException($"Unable to add grade: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToAddGradeException($"Unable to add grade: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all grades.
        /// </summary>
        /// <returns>The list of grade data transfer objects.</returns>
        public async Task<IEnumerable<GradeReturnDTO>> GetAllGrades()
        {
            try
            {
                var grades = (await _gradeRepository.GetAll()).ToList();

                if (grades.Count == 0)
                {
                    throw new NoGradeRecordsExistsException();
                }

                return _mapper.Map<IEnumerable<GradeReturnDTO>>(grades);
            }
            catch (NoGradeRecordsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoGradeRecordsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve all grades: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves a grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the grade.</param>
        /// <returns>The grade data transfer object.</returns>
        public async Task<GradeReturnDTO> GetGradeById(int gradeId)
        {
            try
            {
                var grade = await _gradeRepository.GetById(gradeId);
                if (grade == null)
                {
                    throw new NoSuchGradeRecordExistsException($"Grade with ID {gradeId} does not exist.");
                }
                return _mapper.Map<GradeReturnDTO>(grade);
            }
            catch (NoSuchGradeRecordExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchGradeRecordExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve grade with ID {gradeId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves grades for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The list of grade data transfer objects for the course.</returns>
        public async Task<IEnumerable<GradeReturnDTO>> GetCourseGrades(int courseId)
        {
            try
            {
                var couse = await _courseRepository.GetById(courseId);
                if (couse == null)
                {
                    throw new NoSuchCourseExistException($"Student with ID {courseId} does not exist.");
                }
                var allGrades = await _gradeRepository.GetAll();
                var courseGrades = allGrades.Where(g => g.Exam.CourseId == courseId).ToList();

                if (courseGrades.Count == 0)
                {
                    throw new NoGradeRecordsExistsException();
                }

                return _mapper.Map<IEnumerable<GradeReturnDTO>>(courseGrades);
            }
            catch (NoGradeRecordsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoGradeRecordsExistsException(ex.Message);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve grades for course with ID {courseId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves grades for a specific student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The list of grade data transfer objects for the student.</returns>
        public async Task<IEnumerable<GradeReturnDTO>> GetStudentGrades(int studentId)
        {
            try
            {
                var student = await _studentRepository.GetById(studentId);
                var allGrades = await _gradeRepository.GetAll();
                var studentGrades = allGrades.Where(g => g.StudentId == studentId).ToList();
                if (studentGrades.Count == 0)
                {
                    throw new NoGradeRecordsExistsException();
                }
                return _mapper.Map<IEnumerable<GradeReturnDTO>>(studentGrades);
            }
            catch (NoGradeRecordsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoGradeRecordsExistsException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve grades for student with ID {studentId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the details of a grade.
        /// </summary>
        /// <param name="gradeId">The ID of the grade to update.</param>
        /// <param name="gradeUpdateDTO">The data transfer object containing updated grade details.</param>
        /// <returns>The updated grade data transfer object.</returns>
        /// <exception cref="NoSuchGradeExistException">Thrown when the grade with the given ID does not exist.</exception>
        /// <exception cref="NoSuchFacultyExistException">Thrown when the faculty with the given ID does not exist.</exception>
        /// <exception cref="InvalidMarksScoredException">Thrown when the marks scored exceed the exam total mark.</exception>
        /// <exception cref="UnableToUpdateGradeException">Thrown when unable to update the grade due to an unexpected error.</exception>
        public async Task<GradeReturnDTO> UpdateGrade(int gradeId, GradeUpdateDTO gradeUpdateDTO)
        {
            try
            {
                // Check if the grade exists
                var existingGrade = await _gradeRepository.GetById(gradeId);
                if (existingGrade == null)
                {
                    throw new NoSuchGradeRecordExistsException($"Grade with ID {gradeId} does not exist.");
                }

                // Check if the faculty exists
                var facultyExists = await _facultyRepository.GetById(gradeUpdateDTO.EvaluatedById);
                if (facultyExists == null)
                {
                    throw new NoSuchFacultyExistException($"Faculty with ID {gradeUpdateDTO.EvaluatedById} does not exist.");
                }

                // Check if marks scored is not greater than the exam total mark
                var exam = await _examRepository.GetById(existingGrade.ExamId);
                if (gradeUpdateDTO.MarksScored > exam.TotalMark)
                {
                    throw new InvalidMarksScoredException("Marks scored cannot be greater than the exam total mark.");
                }

                // Calculate percentage and grade
                double percentage = CalculatePercentage(gradeUpdateDTO.MarksScored, exam.TotalMark);
                GradeType gradeType = CalculateGrade(percentage);

                // Update the grade
                existingGrade.EvaluatedById = gradeUpdateDTO.EvaluatedById;
                existingGrade.MarksScored = gradeUpdateDTO.MarksScored;
                existingGrade.Percentage = percentage;
                existingGrade.StudentGrade = gradeType;
                existingGrade.Comments = gradeUpdateDTO.Comments;

                var updatedGrade = await _gradeRepository.Update(existingGrade);
                return _mapper.Map<GradeReturnDTO>(updatedGrade);
            }
            catch (NoSuchGradeRecordExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchGradeRecordExistsException($"Unable to update grade: {ex.Message}");
            }
            catch (NoSuchFacultyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchFacultyExistException($"Unable to update grade: {ex.Message}");
            }
            catch (InvalidMarksScoredException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidMarksScoredException($"Unable to update grade: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateGradeException($"Unable to update grade: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the grade to delete.</param>
        /// <returns>The deleted grade data transfer object.</returns>
        public async Task<GradeReturnDTO> DeleteGrade(int gradeId)
        {
            try
            {
                var grade = await _gradeRepository.GetById(gradeId);
                if (grade == null)
                {
                    throw new NoSuchGradeRecordExistsException($"Grade with ID {gradeId} does not exist.");
                }

                await _gradeRepository.Delete(gradeId);
                return _mapper.Map<GradeReturnDTO>(grade);
            }
            catch (NoSuchGradeRecordExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchGradeRecordExistsException($"Unable to delete grade: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToDeleteGradeException($"Unable to delete grade: {ex.Message}");
            }
        }


        #endregion

        #region Private Methods


        /// <summary>
        /// Validates the provided grade data transfer object (DTO).
        /// </summary>
        /// <param name="gradeDTO">The grade data transfer object containing grade details to be validated.</param>
        /// <exception cref="NoSuchStudentExistException">Thrown when the student with the provided ID does not exist.</exception>
        /// <exception cref="NoSuchExamExistException">Thrown when the exam with the provided ID does not exist.</exception>
        /// <exception cref="StudentNotOptedForCourseException">Thrown when the student has not opted for the course corresponding to the exam.</exception>
        /// <exception cref="NoSuchFacultyExistException">Thrown when the faculty with the provided ID does not exist.</exception>
        /// <exception cref="InvalidMarksScoredException">Thrown when the marks scored are greater than the total mark of the exam.</exception>
        /// <exception cref="InvalidExamDateException">Thrown when attempting to grade an exam that has not yet been completed.</exception>
        private async Task ValidateGradeDTO(GradeDTO gradeDTO)
        {
            // Check if the student exists
            var student = await _studentRepository.GetById(gradeDTO.StudentId);
            if (student == null)
            {
                throw new NoSuchStudentExistException($"Student with ID {gradeDTO.StudentId} does not exist.");
            }

            // Check if the exam exists
            var exam = await _examRepository.GetById(gradeDTO.ExamId);
            if (exam == null)
            {
                throw new NoSuchExamExistException($"Exam with ID {gradeDTO.ExamId} does not exist.");
            }

            if(gradeDTO.StudentId  == student.StudentRollNo && 
                exam.ExamId == gradeDTO.ExamId)
            {
                throw new InvalidGradeException($"Already grade allocated for the student {student.StudentRollNo} for this exam {exam.ExamId}");
            }

            // Check if the student has opted for the course corresponding to the exam
            bool isOptedForCourse = await IsStudentOptedForCourse(gradeDTO.StudentId, exam.CourseId);
            if (!isOptedForCourse)
            {
                throw new StudentNotOptedForCourseException($"Student with ID {gradeDTO.StudentId} has not opted for the course corresponding to the exam.");
            }

            // Check if the faculty exists
            var faculty = await _facultyRepository.GetById(gradeDTO.EvaluatedById);
            if (faculty == null)
            {
                throw new NoSuchFacultyExistException($"Faculty with ID {gradeDTO.EvaluatedById} does not exist.");
            }

            // Check if the marks scored is not greater than the total mark of the exam
            if (gradeDTO.MarksScored > exam.TotalMark)
            {
                throw new InvalidMarksScoredException($"Marks scored cannot be greater than the total mark of the exam ({exam.TotalMark}).");
            }

            // Check if the exam date is in the future
            // just for testing purpose, to be changed to DateTime.Now.Date
            if (exam.ExamDate.Date > CURRENT_DATE_TIME.Date)
            {
                throw new InvalidExamDateException($"Grades cannot be given for exams that haven't completed yet.");
            }



        }


        /// <summary>
        /// Calculates the percentage based on the marks scored and the total marks.
        /// </summary>
        /// <param name="marksScored">The marks scored by the student.</param>
        /// <param name="totalMark">The total marks for the exam.</param>
        /// <returns>The calculated percentage.</returns>
        private double CalculatePercentage(int marksScored, int totalMark)
        {
            // Calculation of percentage based on marks scored and total marks
            return (double)marksScored / totalMark * 100;
        }

        /// <summary>
        /// Calculates the grade based on the percentage obtained.
        /// </summary>
        /// <param name="percentage">The percentage obtained by the student.</param>
        /// <returns>The calculated grade type.</returns>
        private GradeType CalculateGrade(double percentage)
        {
            // Determine the grade based on the percentage obtained
            if (percentage >= 91)
            {
                return GradeType.O;
            }
            else if (percentage >= 81)
            {
                return GradeType.A_Plus;
            }
            else if (percentage >= 71)
            {
                return GradeType.A;
            }
            else if (percentage >= 61)
            {
                return GradeType.B_Plus;
            }
            else if (percentage >= 51)
            {
                return GradeType.B;
            }
            else if (percentage >= 40)
            {
                return GradeType.C;
            }
            else if (percentage < 40 && percentage >= 0)
            {
                return GradeType.F;
            }
            else
            {
                // Handle cases where percentage is negative or exceeds 100
                // You may throw an exception or handle it based on your requirements
                throw new InvalidGradeException("Invalid percentage calculation.");
            }
        }

        /// <summary>
        /// Checks if a student has opted for a specific course.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the student has opted for the course; otherwise, false.</returns>
        private async Task<bool> IsStudentOptedForCourse(int studentId, int courseId)
        {
            // Retrieve all course registrations for the student and filter them based on course approval
            var courseRegistrations = (await _courseRegistrationRepository
                .GetAll())
                .Where(cr => cr.StudentId == studentId)
                .Where(cr => cr.IsApproved)
                .ToList();

            // Check if there is any course registration for the specified course
            return courseRegistrations.Any(cr => cr.CourseId == courseId);
        }

        public async Task<IEnumerable<GradeReturnDTO>> GetTopStudentsByCourse(int courseId)
        {
            try
            {
                var couse = await _courseRepository.GetById(courseId);

                var grades = (await GetCourseGrades(courseId)).OrderByDescending(g => g.Percentage).Take(5).ToList();

                if(grades.Count == 0)
                {
                    throw new NoGradeRecordsExistsException();
                }
                return grades;

            }
            catch (NoGradeRecordsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoGradeRecordsExistsException(ex.Message);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
