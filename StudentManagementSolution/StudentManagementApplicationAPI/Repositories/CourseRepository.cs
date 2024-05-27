using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.CourseExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Models.Db_Models;
using System;

namespace StudentManagementApplicationAPI.Repositories
{
    public class CourseRepository : IRepository<int, Course>
    {
        #region Fields
        private readonly StudentManagementContext _context;
        #endregion

        #region Constructor
        #region Summary
        /// <summary>
        /// Initializes a new instance of the <see ref="StudentManagementContext"/> context.
        /// </summary>
        /// <param name="context">The database context.</param>
        #endregion
        public CourseRepository(StudentManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        #region Adds a Course
        #region Summary
        /// <summary>
        /// Adds a new course in the database.
        /// </summary>
        /// <param name="item">The course object to be added.</param>
        /// <returns>Returns the newly added course object.</returns>
        #endregion
        public async Task<Course> Add(Course item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
                throw new UnableToAddCourseException($"Something went wrong while adding a course with ID {item.CourseId}");
            return item;
        }
        #endregion

        #region Deletes a Course
        #region Summary
        /// <summary\>
        /// Delete a existing course in the database\.
        /// </summary\>
        /// <param name\="key"\>The course primary key that's to be deleted\.</param\>
        /// <returns\>Returns the deleted course object\.</returns\>
        #endregion
        public async Task<Course> Delete(int key)
        {
            var course = await GetById(key);
            if (course == null)
            throw new NoSuchCourseExistException($"Course with given ID {key} doesn't exist!");

            _context.Remove(course);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                ? throw new UnableToDeleteCourseException($"Something went wrong while deleting a course with ID {course.CourseId}")
                : course;
        }
        #endregion

        #region Get all Courses
        #region Summary
        /// <summary\>
        /// To get all the courses in the database\.
        /// </summary\>
        /// <returns\>Returns all the courses in the database\.</returns\>
        #endregion
        public async Task<IEnumerable<Course>> GetAll()
        {
            var courses = await _context.Courses.Include(c => c.Faculty)
                                                .Include(c => c.CourseRegistrations)
                                                .Include(c => c.Exams)
                                                .ToListAsync();
            return  courses;
        }
        #endregion

        #region Get a Specific Course By Id
        #region Summary
        /// <summary>
        /// Gets a specific course that matches the given ID.
        /// </summary>
        /// <param name="key">The course ID.</param>
        /// <returns>Returns the course object with the matching ID.</returns>
        #endregion  
        public async Task<Course> GetById(int key)
        {
            var course = await _context.Courses.Include(c => c.Faculty)
                                               .Include(c => c.CourseRegistrations)
                                               .Include(c => c.Exams)
                                               .FirstOrDefaultAsync(c => c.CourseId == key);

            return course ?? throw new NoSuchCourseExistException($"Course with given ID {key} doesn't exist!");
        }

        #endregion

        #region Update a Course

        #region Summary
        /// <summary>
        /// Updates an existing course in the database.
        /// </summary>
        /// <param name="item">The course object to be updated.</param>
        /// <returns>Returns the updated course object.</returns>
        #endregion
        public async Task<Course> Update(Course item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var course = await GetById(item.CourseId);

            if (course == null)
            {
                throw new NoSuchCourseExistException($"Course with given ID {item.CourseId} doesn't exist!");
            }

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateCourseException($"Something went wrong while updating a course with ID {course.CourseId}")
                : course;
        }

        #endregion

        #endregion
    }
}
