using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Interfaces.Service;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.CourseRegistrationDTOs;

namespace StudentManagementApplicationAPI.Services.Course_Service
{
    public class CourseRegistrationService : ICourseRegistrationService
    {
        #region Private Fields

        private readonly IRepository<int, CourseRegistration> _courseRegistrationRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly IRepository<int, Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseRegistrationService> _logger;

        #endregion

        #region Constructor
        /// <summary>
        /// Injecting the services
        /// </summary>
        /// <param name="courseRegistrationRepository">Course Registration Repository</param>
        /// <param name="courseRepository">Course Repository</param>
        /// <param name="studentRepository">Student Repository</param>
        /// <param name="mapper">Mapper</param>
        public CourseRegistrationService(
            IRepository<int, CourseRegistration> courseRegistrationRepository,
            IRepository<int, Course> courseRepository,
            IRepository<int, Student> studentRepository,
            IMapper mapper,
            ILogger<CourseRegistrationService> logger)
        {
            _courseRegistrationRepository = courseRegistrationRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new course registration.
        /// </summary>
        /// <param name="courseRegistrationAddDTO">The data transfer object containing course registration details.</param>
        /// <returns>The added course registration data transfer object.</returns>
        public async Task<CourseRegistrationReturnDTO> AddCourse(CourseRegistrationAddDTO courseRegistrationAddDTO)
        {
            try
            {
                var student = await _studentRepository.GetById(courseRegistrationAddDTO.StudentId);


                var course = await _courseRepository.GetById(courseRegistrationAddDTO.CourseId);


                if(student.EWallet < course.CourseFees)
                {
                    throw new InsufficientWallentBalanceException("Not enough balance in the student wallet");
                }    

                if(course.CourseVacancy <= 0)
                {
                    throw new InsufficientVacancyException($"Not enough vacancy available for the course {course.CourseId}");
                }


                // Check if the student is already registered for the course
                var alreadyRegistered = (await _courseRegistrationRepository.GetAll())
                                        .Any(cr => cr.CourseId == courseRegistrationAddDTO.CourseId && cr.StudentId == courseRegistrationAddDTO.StudentId);
                if (alreadyRegistered)
                {
                    throw new StudentAlreadyRegisteredForCourseException($"Student with ID {courseRegistrationAddDTO.StudentId} is already registered for course with ID {courseRegistrationAddDTO.CourseId}.");
                }

                var courseRegistration = _mapper.Map<CourseRegistration>(courseRegistrationAddDTO);
                courseRegistration.Comments = "Registered! Yet to be Approved!";
                courseRegistration.IsApproved = false;
                await _courseRegistrationRepository.Add(courseRegistration);

                course.CourseVacancy--;
                await _courseRepository.Update(course);


                student.EWallet -= course.CourseFees;
                await _studentRepository.Update(student);

                return _mapper.Map<CourseRegistrationReturnDTO>(courseRegistration);
            }
            catch (InsufficientVacancyException ex)
            {
                _logger.LogError(ex.Message);
                throw new InsufficientVacancyException(ex.Message);
            }
            catch (InsufficientWallentBalanceException ex)
            {
                _logger.LogError(ex.Message);
                throw new InsufficientWallentBalanceException(ex.Message);
            }
            catch (StudentAlreadyRegisteredForCourseException ex)
            {
                _logger.LogError(ex.Message);
                throw new StudentAlreadyRegisteredForCourseException(ex.Message);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToAddCourseRegistrationException($"Unable to add course registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all course registrations.
        /// </summary>
        /// <returns>The list of course registration data transfer objects.</returns>
        public async Task<IEnumerable<CourseRegistrationReturnDTO>> GetAllCourseRegistrations()
        {
            try
            {
                var courseRegistrations = (await _courseRegistrationRepository.GetAll()).ToList();

                if (courseRegistrations.Count == 0)
                {
                    throw new NoCourseRegistrationsExistsException();
                }

                return _mapper.Map<IEnumerable<CourseRegistrationReturnDTO>>(courseRegistrations);
            }
            catch (NoCourseRegistrationsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoCourseRegistrationsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve course registrations: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration.</param>
        /// <returns>The course registration data transfer object.</returns>
        public async Task<CourseRegistrationReturnDTO> GetCourseRegistrationById(int courseRegistrationId)
        {
            try
            {
                var courseRegistration = await _courseRegistrationRepository.GetById(courseRegistrationId);

                return _mapper.Map<CourseRegistrationReturnDTO>(courseRegistration);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseRegistrationExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve course registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration.</param>
        /// <param name="courseRegistrationAddDTO">The data transfer object containing updated course registration details.</param>
        /// <returns>The updated course registration data transfer object.</returns>
        public async Task<CourseRegistrationReturnDTO> UpdateCourseRegistraion(int courseRegistrationId, int courseId)
        {
            try
            {
                var courseRegistration = await _courseRegistrationRepository.GetById(courseRegistrationId);

                Student student = await _studentRepository.GetById(courseRegistration.StudentId);
                Course OldCourse = await _courseRepository.GetById(courseRegistration.CourseId);
                

                var course = await _courseRepository.GetById(courseId);

                if (courseRegistration.IsApproved)
                {
                    throw new StudentAlreadyApprovedForCourseException($"Previously registered course with id {courseRegistration.CourseId} already approved! Cannot be update once it is approved!");
                }

                // Check if the student has already been approved for the same course
                var existingApprovedRegistration = (await _courseRegistrationRepository.GetAll())
                                                   .FirstOrDefault(cr => cr.CourseId == courseId && cr.StudentId == courseRegistration.StudentId);
                if (existingApprovedRegistration != null)
                {
                    throw new StudentAlreadyRegisteredForCourseException($"Student with ID {courseRegistration.StudentId} is already registered for course with ID {courseId}.");
                }

                // Mappings
                courseRegistration.CourseId = courseId;
                courseRegistration.IsApproved = false;
                courseRegistration.Comments = "Updated! Yet to be approved";

                student.EWallet += OldCourse.CourseFees;
                OldCourse.CourseVacancy++;


                if (student.EWallet < course.CourseFees)
                {
                    throw new InsufficientWallentBalanceException();
                }

                if(course.CourseVacancy <= 0)
                {
                    throw new InsufficientVacancyException();
                }

                student.EWallet -= course.CourseFees;
                course.CourseVacancy--;

                await _studentRepository.Update(student);
                await _courseRepository.Update(OldCourse);
                await _courseRepository.Update(course);
                await _courseRegistrationRepository.Update(courseRegistration);
                return _mapper.Map<CourseRegistrationReturnDTO>(courseRegistration);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseRegistrationExistException(ex.Message);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (StudentAlreadyRegisteredForCourseException ex)
            {
                _logger.LogError(ex.Message);
                throw new StudentAlreadyRegisteredForCourseException(ex.Message);
            }
            catch (StudentAlreadyApprovedForCourseException ex)
            {
                _logger.LogError(ex.Message);
                throw new StudentAlreadyApprovedForCourseException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateCourseRegistrationException($"Unable to update course registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration.</param>
        /// <returns>The deleted course registration data transfer object.</returns>
        public async Task<CourseRegistrationReturnDTO> DeleteCourseRegistration(int courseRegistrationId)
        {
            try
            {
                var courseRegistration = await _courseRegistrationRepository.GetById(courseRegistrationId);


                if (courseRegistration.IsApproved)
                {
                    throw new UnableToDeleteCourseRegistrationException("Once the registration is approved, the registration cannot be deleted");
                }

                

                courseRegistration.Comments = "Registration Deleted!";

                var student = await _studentRepository.GetById(courseRegistration.StudentId);
                var course = await _courseRepository.GetById(courseRegistration.CourseId);
                student.EWallet += course.CourseFees;

                course.CourseVacancy++;

                await _courseRegistrationRepository.Delete(courseRegistrationId);

                await _studentRepository.Update(student);
                await _courseRepository.Update(course);

                return _mapper.Map<CourseRegistrationReturnDTO>(courseRegistration);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseRegistrationExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToDeleteCourseRegistrationException($"Unable to delete course registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the courses registered by a student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The list of course registration data transfer objects.</returns>
        public async Task<IEnumerable<CourseRegistrationReturnDTO>> GetCoursesRegisteredByStudent(int studentId)
        {
            try
            {
                var studentExists = await _studentRepository.GetById(studentId);


                var courseRegistrations = (await _courseRegistrationRepository.GetAll())
                    .Where(cr => cr.StudentId == studentId)
                    .ToList();

                if (courseRegistrations.Count == 0)
                {
                    throw new NoCourseRegistrationsExistsException($"The Student with {studentId} has no course registrations"); ;
                }

                return _mapper.Map<IEnumerable<CourseRegistrationReturnDTO>>(courseRegistrations);
            }
            catch (NoCourseRegistrationsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoCourseRegistrationsExistsException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve courses for student: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the registered students for a course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The list of course registration data transfer objects.</returns>
        public async Task<IEnumerable<CourseRegistrationReturnDTO>> GetRegisteredStudentsForCourse(int courseId)
        {
            try
            {
                var courseExists = await _courseRepository.GetById(courseId);


                var courseRegistrations = (await _courseRegistrationRepository.GetAll())
                    .Where(cr => cr.CourseId == courseId)
                    .ToList();

                if (courseRegistrations.Count == 0)
                {
                    throw new NoCourseRegistrationsExistsException($"No students has registered for the course with id {courseId}");
                }

                return _mapper.Map<IEnumerable<CourseRegistrationReturnDTO>>(courseRegistrations);
            }
            catch (NoCourseRegistrationsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoCourseRegistrationsExistsException(ex.Message);
            }
            catch (NoSuchCourseExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception($"Unable to retrieve students for course: {ex.Message}");
            }
        }

        /// <summary>
        /// Approves a course registration by its ID.
        /// </summary>
        /// <param name="courseRegistrationId">The ID of the course registration.</param>
        /// <returns>The approved course registration data transfer object.</returns>
        public async Task<CourseRegistrationReturnDTO> ApproveCourseRegistrations(int courseRegistrationId)
        {
            try
            {
                var courseRegistration = await _courseRegistrationRepository.GetById(courseRegistrationId);

                if (courseRegistration.IsApproved == true)
                {
                    throw new CourseRegistrationAlreadyApprovedException($"The course registration with Id {courseRegistrationId} already approved");
                }
                courseRegistration.IsApproved = true;
                courseRegistration.Comments = "Approved";
                await _courseRegistrationRepository.Update(courseRegistration);
                return _mapper.Map<CourseRegistrationReturnDTO>(courseRegistration);
            }
            catch (CourseRegistrationAlreadyApprovedException ex)
            {
                _logger.LogError(ex.Message);
                throw new CourseRegistrationAlreadyApprovedException(ex.Message);
            }
            catch (NoSuchCourseRegistrationExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchCourseRegistrationExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateCourseRegistrationException($"Unable to approve course registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Approves all course registrations for a student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The list of approved course registration data transfer objects.</returns>
        public async Task<IEnumerable<CourseRegistrationReturnDTO>> ApproveCourseRegistrationsForStudent(int studentId)
        {
            try
            {
                var studentExists = await _studentRepository.GetById(studentId);


                var courseRegistrations = (await _courseRegistrationRepository.GetAll())
                    .Where(cr => cr.StudentId == studentId && !cr.IsApproved)
                    .ToList();

                if (courseRegistrations.Count == 0)
                {
                    throw new NoCourseRegistrationsExistsException($"There is no course registrations to be approved for student with Roll No {studentId}");
                }

                foreach (var registration in courseRegistrations)
                {
                    registration.IsApproved = true;
                    registration.Comments = "Approved";
                    await _courseRegistrationRepository.Update(registration);
                }

                return _mapper.Map<IEnumerable<CourseRegistrationReturnDTO>>(courseRegistrations);
            }
            catch (NoCourseRegistrationsExistsException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoCourseRegistrationsExistsException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToUpdateCourseRegistrationException($"Unable to approve course registrations for student: {ex.Message}");
            }
        }

        #endregion
    }
}
