using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.StudentAttendanceExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Repositories
{
    public class StudentAttendanceRepository : IRepository<int, StudentAttendance>
    {
        #region Fields

        private readonly StudentManagementContext _context;

        #endregion

        #region Constructor

        #region Summary
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentManagementContext"/> context.
        /// </summary>
        /// <param name="context">The database context.</param>
        #endregion
        public StudentAttendanceRepository(StudentManagementContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Add StudentAttendance

        #region Summary
        /// <summary>
        /// Adds a new student attendance record to the database.
        /// </summary>
        /// <param name="item">The student attendance object to be added.</param>
        /// <returns>Returns the newly added student attendance object.</returns>
        #endregion
        public async Task<StudentAttendance> Add(StudentAttendance item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
            {
                throw new UnableToAddStudentAttendanceException($"Something went wrong while adding a student attendance record with ID {item.ID}");
            }

            return item;
        }

        #endregion

        #region Delete StudentAttendance

        #region Summary
        /// <summary>
        /// Deletes an existing student attendance record from the database.
        /// </summary>
        /// <param name="key">The student attendance ID to be deleted.</param>
        /// <returns>Returns the deleted student attendance object.</returns>
        #endregion
        public async Task<StudentAttendance> Delete(int key)
        {
            var attendance = await GetById(key);

            if (attendance == null)
            {
                throw new NoSuchStudentAttendanceExistException($"Student attendance record with ID {key} doesn't exist!");
            }

            _context.Remove(attendance);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToDeleteStudentAttendanceException($"Something went wrong while deleting a student attendance record with ID {attendance.ID}")
                : attendance;
        }

        #endregion

        #region Get All StudentAttendances

        #region Summary
        /// <summary>
        /// Gets all student attendance records from the database.
        /// </summary>
        /// <returns>Returns all student attendance records as an IEnumerable collection.</returns>
        #endregion
        public async Task<IEnumerable<StudentAttendance>> GetAll()
        {
            var attendances = await _context.StudentAttendances.Include(a=>a.Student)
                                                            .Include(a=>a.Course)
                                                            .ToListAsync();

            return attendances;
        }

        #endregion

        #region Get StudentAttendance By Id

        #region Summary
        /// <summary>
        /// Gets a specific student attendance record that matches the given ID.
        /// </summary>
        /// <param name="key">The student attendance ID.</param>
        /// <returns>Returns the student attendance object with the matching ID.</returns>
        #endregion
        public async Task<StudentAttendance> GetById(int key)
        {
            var attendance = await _context.StudentAttendances.Include(a => a.Student)
                                                            .Include(a => a.Course).FirstOrDefaultAsync(a => a.ID == key);

            return attendance == null
                ? throw new NoSuchStudentAttendanceExistException($"Student attendance record with ID {key} doesn't exist!")
                : attendance;
        }

        #endregion

        #region Update StudentAttendance

        #region Summary
        /// <summary>
        /// Updates an existing student attendance record in the database.
        /// </summary>
        /// <param name="item">The student attendance object to be updated.</param>
        /// <returns>Returns the updated student attendance object.</returns>
        #endregion
        public async Task<StudentAttendance> Update(StudentAttendance item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var attendance = await GetById(item.ID);

            if (attendance == null)
            {
                throw new NoSuchStudentAttendanceExistException($"Student attendance record with ID {item.ID} doesn't exist!");
            }

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateStudentAttendanceException($"Something went wrong while updating a student attendance record with ID {attendance.ID}")
                : item;
        }

        #endregion

        #endregion
    }

}
