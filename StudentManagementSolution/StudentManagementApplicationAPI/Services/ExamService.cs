using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.ExamDTOs;
using StudentManagementApplicationAPI.Models.Enums;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services
{
    public class ExamService : IExamService
    {
        #region Private Fields

        private readonly IRepository<int, Exam> _examRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ExamService(IRepository<int, Exam> examRepository, IRepository<int, Course> courseRepository,
            IMapper mapper)
        {
            _examRepository = examRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new exam.
        /// </summary>
        /// <param name="examDTO">The data transfer object containing exam details.</param>
        /// <returns>The added exam data transfer object.</returns>
        public async Task<ExamReturnDTO> AddExam(ExamDTO examDTO)
        {
            try
            {
                await ValidateExam(examDTO);

                var existingExam = await GetFutureExamByCourseId(examDTO.CourseId);
                if (existingExam != null)
                {
                    throw new ExamAlreadyScheduledException("An exam for this course is already scheduled in the future.");
                }


                

                //var exam = _mapper.Map<Exam>(examDTO);
                Exam exam = MapExamDtoToExam(examDTO);
                

                var addedExam = await _examRepository.Add(exam);
                return _mapper.Map<ExamReturnDTO>(addedExam);
            }
            catch (NoSuchCourseExistException ex)
            {
                throw new NoSuchCourseExistException($"Unable to add exam: {ex.Message}");
            }
            catch (ExamAlreadyScheduledException ex)
            {
                throw new ExamAlreadyScheduledException($"Unable to add exam: {ex.Message}");
            }
            catch (InvalidTotalMarkException ex)
            {
                throw new InvalidTotalMarkException($"Unable to add exam: {ex.Message}");
            }
            catch (InvalidExamDateException ex)
            {
                throw new InvalidExamDateException($"Unable to add exam: {ex.Message}");
            }
            catch (InvalidExamTypeException ex)
            {
                throw new InvalidExamTypeException($"Unable to add exam: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new UnableToAddExamException($"Unable to add exam: {ex.Message}");
            }
        }

        /// <summary>
        /// Mapper function that maps ExamDTO to Exam
        /// </summary>
        /// <param name="examDTO">ExamDTO object to be mapped</param>
        /// <returns></returns>
        /// <exception cref="InvalidExamTypeException"></exception>
        private Exam MapExamDtoToExam(ExamDTO examDTO)
        {
            var startTime = new DateTime(examDTO.ExamDate.Year, examDTO.ExamDate.Month, examDTO.ExamDate.Day)
                                        .Add(examDTO.StartTime.ToTimeSpan());

            var endTime = new DateTime(examDTO.ExamDate.Year, examDTO.ExamDate.Month, examDTO.ExamDate.Day)
                .Add(examDTO.EndTime.ToTimeSpan());
            var exam = new Exam()
            {
                CourseId = examDTO.CourseId,
                ExamDate = examDTO.ExamDate.ToDateTime(TimeOnly.MinValue),
                TotalMark = examDTO.TotalMark,
                StartTime = startTime,
                EndTime = endTime
            };
            if (Enum.TryParse<ExamType>(examDTO.ExamType.ToLower(), true, out var examTypeEnum))
            {
                exam.ExamType = examTypeEnum;
            }
            else
            {
                // Handle the case where the string value doesn't match any enum value
                throw new InvalidExamTypeException("Invalid exam type provided.");
            }
            return exam;
        }


        /// <summary>
        /// Validates the properties of an exam DTO.
        /// </summary>
        /// <param name="examDTO">The exam DTO to validate.</param>
        /// <exception cref="NoSuchCourseExistException">Thrown if the specified course ID does not exist.</exception>
        /// <exception cref="InvalidTotalMarkException">Thrown if the total mark is not between 1 and 100.</exception>
        /// <exception cref="InvalidExamDateException">Thrown if the exam date is not in the future.</exception>
        /// <exception cref="InvalidExamTypeException">Thrown if the exam type is not 'Online' or 'Offline'.</exception>
        private async Task ValidateExam(ExamDTO examDTO)
        {
            // Validate CourseId
            var courseExists = await _courseRepository.GetById(examDTO.CourseId);
            if (courseExists == null)
            {
                throw new NoSuchCourseExistException($"Course with ID {examDTO.CourseId} does not exist.");
            }


            // Validate TotalMark
            if (examDTO.TotalMark < 1 || examDTO.TotalMark > 100)
            {
                throw new InvalidTotalMarkException("TotalMark must be between 1 and 100.");
            }

            // Validate ExamDate
            if (examDTO.ExamDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now.Date)
            {
                throw new InvalidExamDateException("ExamDate must be in the future.");
            }

            // Validate ExamType
            if (!Enum.IsDefined(typeof(ExamType), examDTO.ExamType))
            {
                throw new InvalidExamTypeException("ExamType must be either 'Online' or 'Offline'.");
            }
        }


        /// <summary>
        /// Retrieves the first future exam for the specified course ID from the repository.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>
        /// The first future exam for the specified course ID if found; otherwise, null.
        /// </returns>
        private async Task<Exam> GetFutureExamByCourseId(int courseId)
        {
            // Retrieve all exams from the repository and filter by course ID and future date
            var exams = (await _examRepository.GetAll()).ToList();

            var futureExam = exams.FirstOrDefault(e => e.CourseId == courseId && e.ExamDate > DateTime.Now);

            return futureExam;
                
        }


        /// <summary>
        /// Retrieves all exams.
        /// </summary>
        /// <returns>The list of exam data transfer objects.</returns>
        public async Task<IEnumerable<ExamReturnDTO>> GetAllExams()
        {
            try
            {
                var exams = (await _examRepository.GetAll()).ToList();
                if (exams.Count == 0)
                {
                    throw new NoExamsExistsException();
                }
                return _mapper.Map<IEnumerable<ExamReturnDTO>>(exams);
            }
            catch (NoExamsExistsException ex)
            {
                throw new NoExamsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve exams: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The exam data transfer object.</returns>
        public async Task<ExamReturnDTO> GetExamById(int examId)
        {
            try
            {
                var exam = await _examRepository.GetById(examId);
                if (exam == null)
                {
                    throw new NoSuchExamExistException($"Exam with ID {examId} does not exist.");
                }
                return _mapper.Map<ExamReturnDTO>(exam);
            }
            catch (NoSuchExamExistException ex)
            {
                throw new NoSuchExamExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve exam: {ex.Message}");
            }
        }


        /// <summary>
        /// Updates an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <param name="examDTO">The data transfer object containing updated exam details.</param>
        /// <returns>The updated exam data transfer object.</returns>
        public async Task<ExamReturnDTO> UpdateExam(int examId, ExamDTO examDTO)
        {
            try
            {
                var exam = await _examRepository.GetById(examId);
                if (exam == null)
                {
                    throw new NoSuchExamExistException($"Exam with ID {examId} does not exist.");
                }

                await ValidateExam(examDTO);

                var exams = (await _examRepository.GetAll()).ToList();

                var existingExam = exams.FirstOrDefault(e => e.CourseId == examDTO.CourseId && e.ExamDate > DateTime.Now &&
                                                        e.ExamId != examId);

                if (existingExam != null)
                {
                    throw new ExamAlreadyScheduledException("An exam for this course is already scheduled in the future.");
                }

                #region Mapping 
                exam.ExamId = examId;
                var startTime = new DateTime(examDTO.ExamDate.Year, examDTO.ExamDate.Month, examDTO.ExamDate.Day)
                                        .Add(examDTO.StartTime.ToTimeSpan());

                var endTime = new DateTime(examDTO.ExamDate.Year, examDTO.ExamDate.Month, examDTO.ExamDate.Day)
                    .Add(examDTO.EndTime.ToTimeSpan());
                exam.CourseId = examDTO.CourseId;
                exam.ExamDate = examDTO.ExamDate.ToDateTime(TimeOnly.MinValue);
                exam.TotalMark = examDTO.TotalMark;
                exam.StartTime = startTime;
                exam.EndTime = endTime;
                #endregion

                if (Enum.TryParse<ExamType>(examDTO.ExamType.ToLower(), true, out var examTypeEnum))
                {
                    exam.ExamType = examTypeEnum;
                }
                else
                {
                    throw new InvalidExamTypeException("Invalid exam type provided.");
                }
                await _examRepository.Update(exam);

                return _mapper.Map<ExamReturnDTO>(exam);
            }
            catch (NoSuchExamExistException ex)
            {
                throw new NoSuchExamExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateExamException($"Unable to update exam: {ex.Message}");
            }
        }


        /// <summary>
        /// Deletes an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The deleted exam data transfer object.</returns>
        public async Task<ExamReturnDTO> DeleteExam(int examId)
        {
            try
            {
                var exam = await _examRepository.GetById(examId);
                if (exam == null)
                {
                    throw new NoSuchExamExistException($"Exam with ID {examId} does not exist.");
                }

                if (exam.ExamDate.Date < DateTime.Now.Date)
                {
                    throw new CannotDeleteFinishedExamException($"Exam with ID {examId} has already finished and cannot be deleted.");
                }

                await _examRepository.Delete(examId);
                return _mapper.Map<ExamReturnDTO>(exam);
            }
            catch (NoSuchExamExistException ex)
            {
                throw new NoSuchExamExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new UnableToDeleteExamException($"Unable to delete exam: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves exams by the specified date.
        /// </summary>
        /// <param name="date">The date of the exams.</param>
        /// <returns>The list of exam data transfer objects.</returns>
        public async Task<IEnumerable<ExamReturnDTO>> GetExamsByDate(DateTime date)
        {
            try
            {
                var exams = (await _examRepository.GetAll()).Where(e => e.ExamDate.Date == date.Date).ToList();
                if (exams.Count == 0)
                {
                    throw new NoExamsExistsException($"No exams exist on date {date.ToShortDateString()}.");
                }
                return _mapper.Map<IEnumerable<ExamReturnDTO>>(exams);
            }
            catch (NoExamsExistsException ex)
            {
                throw new NoExamsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve exams: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves upcoming exams within the specified number of days.
        /// </summary>
        /// <param name="days">The number of days to look ahead for upcoming exams </param>
        /// <returns>The list of exam data transfer objects.</returns>
        public async Task<IEnumerable<ExamReturnDTO>> GetUpcomingExams(int days)
        {
            try
            {
                var startDate = DateTime.Today;
                var endDate = startDate.AddDays(days);

                var exams = (await _examRepository.GetAll())
                    .Where(e => e.ExamDate.Date >= startDate && e.ExamDate.Date < endDate)
                    .OrderBy(e => e.ExamDate)
                    .ToList();

                if (exams.Count == 0)
                {
                    throw new NoSuchExamExistException($"No exams exist within the next {days} days from today.");
                }

                return _mapper.Map<IEnumerable<ExamReturnDTO>>(exams);
            }
            catch (NoSuchExamExistException ex)
            {
                throw new NoSuchExamExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve exams: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves offline exams.
        /// </summary>
        /// <returns>The list of offline exam data transfer objects.</returns>
        public async Task<IEnumerable<ExamReturnDTO>> GetOfflineExams()
        {
            try
            {
                var exams = (await _examRepository.GetAll()).Where(e => e.ExamType == ExamType.Offline).ToList();
                if (exams.Count == 0)
                {
                    throw new NoSuchExamExistException($"No offline exams exist.");
                }
                return _mapper.Map<IEnumerable<ExamReturnDTO>>(exams);
            }
            catch (NoSuchExamExistException ex)
            {
                throw new NoSuchExamExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve offline exams: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves online exams.
        /// </summary>
        /// <returns>The list of online exam data transfer objects.</returns>
        public async Task<IEnumerable<ExamReturnDTO>> GetOnlineExams()
        {
            try
            {
                var exams = (await _examRepository.GetAll()).Where(e => e.ExamType == ExamType.Online).ToList();
                if (exams.Count == 0)
                {
                    throw new NoSuchExamExistException($"No online exams exist.");
                }
                return _mapper.Map<IEnumerable<ExamReturnDTO>>(exams);
            }
            catch (NoSuchExamExistException ex)
            {
                throw new NoSuchExamExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve online exams: {ex.Message}");
            }
        }


        #endregion
    }

}
