using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseRegistrationExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Repositories
{
    public class CourseRegistrationRepository : IRepository<int, CourseRegistration>
    {
        #region Fields

        private readonly StudentManagementContext _context;

        #endregion

        #region Constructor
        #region Summary
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentManagementContext"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        #endregion
        public CourseRegistrationRepository(StudentManagementContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Add a CourseRegistration
        #region Summary
        /// <summary>
        /// Adds a new course registration to the database.
        /// </summary>
        /// <param name="item">The course registration object to be added.</param>
        /// <returns>Returns the newly added course registration object.</returns>
        #endregion
        public async Task<CourseRegistration> Add(CourseRegistration item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
            {
                throw new UnableToAddCourseRegistrationException($"Something went wrong while adding a course registration with ID {item.RegistrationId}");
            }

            return item;
        }

        #endregion

        #region Delete a CourseRegistration
        #region Summary
        /// <summary>
        /// Deletes an existing course registration from the database.
        /// </summary>
        /// <param name="key">The course registration ID to be deleted.</param>
        /// <returns>Returns the deleted course registration object.</returns>
        #endregion
        public async Task<CourseRegistration> Delete(int key)
        {
            var courseRegistration = await GetById(key);

            if (courseRegistration == null)
            {
                throw new NoSuchCourseRegistrationExistException($"Course registration with ID {key} doesn't exist!");
            }

            _context.Remove(courseRegistration);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToDeleteCourseRegistrationException($"Something went wrong while deleting a course registration with ID {courseRegistration.RegistrationId}")
                : courseRegistration;
        }

        #endregion

        #region Get All CourseRegistrations
        #region Summary
        /// <summary>
        /// Gets all course registrations from the database.
        /// </summary>
        /// <returns>Returns all course registration objects as an IEnumerable collection.</returns>
        #endregion
        public async Task<IEnumerable<CourseRegistration>> GetAll()
        {
            var courseRegistrations = await _context.CourseRegistrations
                                                    .Include(cr => cr.Course)
                                                    .Include(cr => cr.Student)
                                                    .ToListAsync();

            return  courseRegistrations;
        }

        #endregion

        #region Get CourseRegistration By Id
        #region Summary
        /// <summary>
        /// Gets a specific course registration that matches the given ID.
        /// </summary>
        /// <param name="key">The course registration ID.</param>
        /// <returns>Returns the course registration object with the matching ID.</returns>
        #endregion
        public async Task<CourseRegistration> GetById(int key)
        {
            var courseRegistration = await _context.CourseRegistrations
                                                   .Include(cr => cr.Course)
                                                   .Include(cr => cr.Student)
                                                   .FirstOrDefaultAsync(cr => cr.RegistrationId == key);

            return courseRegistration == null
                ? throw new NoSuchCourseRegistrationExistException($"Course registration with ID {key} doesn't exist!")
                : courseRegistration;
        }

        #endregion

        #region Update a CourseRegistration
        #region Summary
        /// <summary>
        /// Updates an existing course registration in the database.
        /// </summary>
        /// <param name="item">The course registration object to be updated.</param>
        /// <returns>Returns the updated course registration object.</returns>
        #endregion
        public async Task<CourseRegistration> Update(CourseRegistration item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var courseRegistration = await GetById(item.RegistrationId);

            if (courseRegistration == null)
            {
                throw new NoSuchCourseRegistrationExistException($"Course registration with ID {item.RegistrationId} doesn't exist!");
            }

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateCourseRegistrationException($"Something went wrong while updating a course registration with ID {courseRegistration.RegistrationId}")
                : courseRegistration;
        }

        #endregion

        #endregion
    }

}
