using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.GradeExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces.Repository;
using StudentManagementApplicationAPI.Models.Db_Models;
using System;

namespace StudentManagementApplicationAPI.Repositories
{
    public class GradeRepository : IRepository<int, Grade>
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
        public GradeRepository(StudentManagementContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Add a Grade

        #region Summary
        /// <summary>
        /// Adds a new grade record to the database.
        /// </summary>
        /// <param name="item">The grade object to be added.</param>
        /// <returns>Returns the newly added grade object.</returns>
        #endregion
        public async Task<Grade> Add(Grade item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            
            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
                throw new UnableToAddGradeException($"Something went wrong while adding a grade record.");
            
            return item;
        }
        #endregion

        #region Delete a Grade

        #region Summary
        /// <summary>
        /// Deletes an existing grade record from the database.
        /// </summary>
        /// <param name="key">The grade record ID (GradeId) to be deleted.</param>
        /// <returns>Returns the deleted grade object.</returns>
        #endregion
        public async Task<Grade> Delete(int key)
        {
            var grade = await GetById(key);

            if (grade == null)
                throw new NoSuchGradeRecordExistsException($"Grade record with ID {key} doesn't exist!");
            

            _context.Remove(grade);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToDeleteGradeException($"Something went wrong while deleting a grade record with ID {grade.Id}")
                : grade;
        }

        #endregion

        #region Get All Grades

        #region Summary
        /// <summary>
        /// Gets all grade records from the database.
        /// </summary>
        /// <returns>Returns all grade records as an IEnumerable collection.</returns>
        #endregion
        public async Task<IEnumerable<Grade>> GetAll()
        {
            var grades = await _context.Grades.Include(g => g.Student)
                                            .Include(g => g.EvaluatedBy)
                                            .Include(g => g.Exam)
                                            .ToListAsync();

            return  grades;
        }

        #endregion

        #region Get Grade By Id

        #region Summary
        /// <summary>
        /// Gets a specific grade record that matches the given ID.
        /// </summary>
        /// <param name="key">The grade record ID.</param>
        /// <returns>Returns the grade record object with the matching ID</returns>
        #endregion
        public async Task<Grade> GetById(int key)
        {
            var grade = await _context.Grades.Include(g => g.Student)
                                            .Include(g => g.Student)
                                            .Include(g => g.EvaluatedBy)
                                            .Include(g => g.Exam).FirstOrDefaultAsync(g => g.Id == key);

            return grade == null
                ?
                throw new NoSuchGradeRecordExistsException($"Grade with given Id {key} doesn't exist!")
                :
                grade;
        }

        #endregion

        #region Update a Grade

        #region Summary
        /// <summary>
        /// Updates an existing grade record in the database.
        /// </summary>
        /// <param name="item">The grade object to be updated.</param>
        /// <returns>Returns the updated grade object.</returns>
        #endregion
        public async Task<Grade> Update(Grade item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            
            var gradeToUpdate = await GetById(item.Id);

            if (gradeToUpdate == null)
                throw new NoSuchGradeRecordExistsException($"Grade record with ID {item.Id} doesn't exist!");
            
            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateGradeException($"Something went wrong while updating a grade record with ID {gradeToUpdate.Id}")
                : gradeToUpdate;
        }
        #endregion

        #endregion
    }
}
