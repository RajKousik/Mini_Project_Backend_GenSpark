using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.CourseDTOs;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services
{
    public class CourseService : ICourseService
    {
        #region Private Fields

        private readonly IRepository<int, Course> _courseRepository;
        private readonly IRepository<int, Faculty> _facultyRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CourseService(IRepository<int, Course> courseRepository, IMapper mapper, IRepository<int, Faculty> facultyRepository)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _facultyRepository = facultyRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="courseDTO">The data transfer object containing course details.</param>
        /// <returns>The added course data transfer object.</returns>
        public async Task<CourseReturnDTO> AddCourse(CourseDTO courseDTO)
        {
            try
            {

                var courseExists = (await _courseRepository.GetAll()).Any(c => c.Name == courseDTO.Name);

                if(courseExists) 
                {
                    throw new CourseAlreadyExistsException($"Course with Name {courseDTO.Name} already exists.");
                }

                // Check if the faculty exists
                var facultyExists = (await _facultyRepository.GetAll()).Any(f => f.FacultyId == courseDTO.FacultyId);
                if (!facultyExists)
                {
                    throw new NoSuchFacultyExistException($"Faculty with ID {courseDTO.FacultyId} does not exist.");
                }

                // Map the DTO to the Course entity
                var course = _mapper.Map<Course>(courseDTO);

                // Add the course to the repository
                var addedCourse = await _courseRepository.Add(course);

                var courseReturnDTO = _mapper.Map<CourseReturnDTO>(addedCourse);
                // Map the Course entity to the return DTO
                return courseReturnDTO;
            }
            catch (CourseAlreadyExistsException ex)
            {
                throw new CourseAlreadyExistsException(ex.Message);
            }
            catch (NoSuchFacultyExistException ex)
            {
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new UnableToAddCourseException($"Unable to add course: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all courses.
        /// </summary>
        /// <returns>The list of course data transfer objects.</returns>
        public async Task<IEnumerable<CourseReturnDTO>> GetAllCourses()
        {
            try
            {
                var courses = (await _courseRepository.GetAll()).ToList();
                if( courses.Count == 0)
                {
                    throw new NoCoursesExistsException();
                }
                return _mapper.Map<IEnumerable<CourseReturnDTO>>(courses);
            }
            catch (NoCoursesExistsException ex)
            {
                throw new NoCoursesExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve courses: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The course data transfer object.</returns>
        public async Task<CourseReturnDTO> GetCourseById(int courseId)
        {
            try
            {
                var course = await _courseRepository.GetById(courseId);
                if (course == null)
                {
                    throw new NoSuchCourseExistException($"Course with ID {courseId} does not exist.");
                }
                return _mapper.Map<CourseReturnDTO>(course);
            }
            catch (NoSuchCourseExistException ex)
            {
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve course: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a course by its name.
        /// </summary>
        /// <param name="name">The name of the course.</param>
        /// <returns>The course data transfer object.</returns>
        public async Task<IEnumerable<CourseReturnDTO>> GetCourseByName(string name)
        {
            try
            {
                var courses = (await _courseRepository.GetAll()).Where(c => c.Name.ToLower().Contains(name.ToLower())).ToList();
                if (courses.Count == 0)
                {
                    throw new NoSuchCourseExistException($"Course with name {name} does not exist.");
                }
                return _mapper.Map<IEnumerable<CourseReturnDTO>>(courses);
            }
            catch (NoSuchCourseExistException ex)
            {
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve course: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves courses by the faculty ID.
        /// </summary>
        /// <param name="facultyId">The ID of the faculty.</param>
        /// <returns>The list of course data transfer objects.</returns>
        public async Task<IEnumerable<CourseReturnDTO>> GetCourseByFaculty(int facultyId)
        {
            try
            {
                var courses = (await _courseRepository.GetAll()).Where(c => c.FacultyId == facultyId).ToList();
                if (courses.Count == 0)
                {
                    throw new NoCoursesExistForFacultyException($"No courses exist for faculty with ID {facultyId}.");
                }
                return _mapper.Map<IEnumerable<CourseReturnDTO>>(courses);
            }
            catch (NoCoursesExistForFacultyException ex)
            {
                throw new NoCoursesExistForFacultyException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrieve courses: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="courseDTO">The data transfer object containing updated course details.</param>
        /// <returns>The updated course data transfer object.</returns>
        public async Task<CourseReturnDTO> UpdateCourse(int courseId, CourseDTO courseDTO)
        {
            try
            {
                // Check if the course exists
                var course = await _courseRepository.GetById(courseId);
                if (course == null)
                {
                    throw new NoSuchCourseExistException($"Course with ID {courseId} does not exist.");
                }

                // Check if the new course name already exists
                if (!string.IsNullOrEmpty(courseDTO.Name))
                {
                    var courseWithNameExists = (await _courseRepository.GetAll())
                                                .Any(c => c.Name == courseDTO.Name && c.CourseId != courseId);
                    if (courseWithNameExists)
                    {
                        throw new CourseAlreadyExistsException($"Course with name {courseDTO.Name} already exists.");
                    }
                    course.Name = courseDTO.Name;
                }

                // Update other course properties if provided
                if (!string.IsNullOrEmpty(courseDTO.Description))
                {
                    course.Description = courseDTO.Description;
                }

                // Check if the updated faculty exists
                var facultyExists = (await _facultyRepository.GetAll()).Any(f => f.FacultyId == courseDTO.FacultyId);
                if (!facultyExists)
                {
                    throw new NoSuchFacultyExistException($"Faculty with ID {courseDTO.FacultyId} does not exist.");
                }
                course.FacultyId = courseDTO.FacultyId;

                // Update the course in the repository
                await _courseRepository.Update(course);

                // Map the updated course entity to the return DTO
                return _mapper.Map<CourseReturnDTO>(course);
            }
            catch (NoSuchCourseExistException ex)
            {
                throw new NoSuchCourseExistException(ex.Message);
            }
            catch (CourseAlreadyExistsException ex)
            {
                throw new CourseAlreadyExistsException(ex.Message);
            }
            catch (NoSuchFacultyExistException ex)
            {
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateCourseException($"Unable to update course: {ex.Message}");
            }
        }


        #endregion
    }


}
