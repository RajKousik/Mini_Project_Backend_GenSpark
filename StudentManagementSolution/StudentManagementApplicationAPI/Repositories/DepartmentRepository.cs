using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Repositories
{
    public class DepartmentRepository : IRepository<int, Department>
    {
        #region Fields

        private readonly StudentManagementContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentManagementContext"/> context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public DepartmentRepository(StudentManagementContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Adds a Department

        #region Summary
        /// <summary>
        /// Adds a new department to the database.
        /// </summary>
        /// <param name="item">The department object to be added.</param>
        /// <returns>Returns the newly added department object.</returns>
        #endregion
        public async Task<Department> Add(Department item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
            {
                throw new UnableToAddDepartmentException($"Something went wrong while adding a department with ID {item.DeptId}");
            }

            return item;
        }

        #endregion

        #region Delete a Department
        #region Summary
        /// <summary>
        /// Deletes an existing department from the database.
        /// </summary>
        /// <param name="key">The department Id (DeptId) to be deleted.</param>
        /// <returns>Returns the deleted department object.</returns>
        #endregion
        public async Task<Department> Delete(int key)
        {
            var department = await GetById(key);

            if (department == null)
            {
                throw new NoSuchDepartmentExistException($"Department with ID {key} doesn't exist!");
            }

            _context.Remove(department);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToDeleteDepartmentException($"Something went wrong while deleting a department with ID {department.DeptId}")
                : department;
        }

        #endregion

        #region Get All Departments
        #region Summary
        /// <summary>
        /// Gets all departments from the database.
        /// </summary>
        /// <returns>Returns all departments as an IEnumerable collection.</returns>
        #endregion
        public async Task<IEnumerable<Department>> GetAll()
        {
            var departments = await _context.Departments.Include(d => d.Head)
                                                    .Include(d => d.Students)
                                                    .Include(d => d.Faculties)
                                                    .ToListAsync();

            return departments.Count == 0
                ? throw new NoDepartmentsExistsException("No Departments found in the database!")
                : departments;
        }

        #endregion

        #region Get by Id
        #region Summary
        /// <summary>
        /// Gets a specific department that matches the given ID.
        /// </summary>
        /// <param name="key">The department ID.</param>
        /// <returns>Returns the department object with the matching ID</returns>
        #endregion
        public async Task<Department> GetById(int key)
        {
            var department = await _context.Departments.Include(d => d.Head)
                                                    .Include(d => d.Students)
                                                    .Include(d => d.Faculties)
                                                    .FirstOrDefaultAsync(d => d.DeptId == key);

            if(department == null)
            {
                throw new NoSuchDepartmentExistException($"Department with given ID {key} doesn't exist!");
            }

            return department;
        }

        #endregion

        #region Update a Database

        #region Summary
        /// <summary>
        /// Updates an existing department in the database.
        /// </summary>
        /// <param name="item">The department object to be updated.</param>
        /// <returns>Returns the updated department object.</returns>
        #endregion
        public async Task<Department> Update(Department item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var department = await GetById(item.DeptId);

            if (department == null)
            {
                throw new NoSuchDepartmentExistException($"Department with ID {item.DeptId} doesn't exist!");
            }

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateDepartmentException($"Something went wrong while updating a department with ID {department.DeptId}")
                : department;
        }

        #endregion

        #endregion
    }
}
