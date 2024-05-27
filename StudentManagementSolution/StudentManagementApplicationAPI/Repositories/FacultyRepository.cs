using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Repositories
{
    public class FacultyRepository : IRepository<int, Faculty>
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
        public FacultyRepository(StudentManagementContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Add a Faculty

        #region Summary
        /// <summary>
        /// Adds a new faculty member to the database.
        /// </summary>
        /// <param name="item">The faculty object to be added.</param>
        /// <returns>Returns the newly added faculty object.</returns>
        #endregion
        public async Task<Faculty> Add(Faculty item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
            {
                throw new UnableToAddFacultyException($"Something went wrong while adding a faculty member with ID {item.FacultyId}");
            }

            return item;
        }

        #endregion

        #region Delete a Faculty

        #region Summary
        /// <summary>
        /// Deletes an existing faculty member from the database.
        /// </summary>
        /// <param name="key">The faculty member ID to be deleted.</param>
        /// <returns>Returns the deleted faculty object.</returns>
        #endregion
        public async Task<Faculty> Delete(int key)
        {
            var faculty = await GetById(key); // Don't modify GetById

            if (faculty == null)
            {
                throw new NoSuchFacultyExistException($"Faculty member with ID {key} doesn't exist!");
            }

            _context.Remove(faculty);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToDeleteFacultyException($"Something went wrong while deleting a faculty member with ID {faculty.FacultyId}")
                : faculty;
        }

        #endregion

        #region Get All Faculties

        #region Summary
        /// <summary>
        /// Gets all faculty members from the database.
        /// </summary>
        /// <returns>Returns all faculty members as an IEnumerable collection.</returns>
        #endregion
        public async Task<IEnumerable<Faculty>> GetAll()
        {
            var faculties = await _context.Faculties.Include(f => f.Department)
                                                    .ToListAsync();

            return  faculties;
        }

        #endregion

        #region Get Faculty By Id

        #region Summary
        /// <summary>
        /// Gets a specific faculty member that matches the given ID.
        /// </summary>
        /// <param name="key">The faculty member ID.</param>
        /// <returns>Returns the faculty member object with the matching ID</returns>
        #endregion
        public async Task<Faculty> GetById(int key)
        {
            var faculty = await _context.Faculties.Include(f => f.Department)
                                    .FirstOrDefaultAsync(f => f.FacultyId == key);
            
            return faculty == null
                    ?
                    throw new NoSuchFacultyExistException($"Faculty with given Id {key} doesn't exist!")
                    :
                    faculty;
        }

        #endregion

        #region Update a Faculty

        #region Summary
        /// <summary>
        /// Updates an existing faculty member in the database.
        /// </summary>
        /// <param name="item">The faculty object to be updated.</param>
        /// <returns>Returns the updated faculty object.</returns>
        #endregion
        public async Task<Faculty> Update(Faculty item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var faculty = await GetById(item.FacultyId); 

            if (faculty == null)
            {
                throw new NoSuchFacultyExistException($"Faculty member with ID {item.FacultyId} doesn't exist!");
            }

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateFacultyException($"Something went wrong while updating a faculty member with ID {faculty.FacultyId}")
                : faculty;
        }

        #endregion

        #endregion
    }
}
