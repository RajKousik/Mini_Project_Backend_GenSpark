using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.ExamExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Repositories
{
    public class ExamRepository : IRepository<int, Exam>
    {
        #region Fields

        private readonly StudentManagementContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentManagementContext"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ExamRepository(StudentManagementContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Add an Exam
        #region Summary
        /// <summary>
        /// Adds a new exam to the database.
        /// </summary>
        /// <param name="item">The exam object to be added.</param>
        /// <returns>Returns the newly added exam object.</returns>
        #endregion
        public async Task<Exam> Add(Exam item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if (noOfRowsUpdated <= 0)
            {
                throw new UnableToAddExamException($"Something went wrong while adding an exam with ID {item.ExamId}");
            }

            return item;
        }

        #endregion

        #region Delete an Exam
        #region Summary
        /// <summary>
        /// Deletes an existing exam from the database.
        /// </summary>
        /// <param name="key">The exam ID to be deleted.</param>
        /// <returns>Returns the deleted exam object.</returns>
        #endregion
        public async Task<Exam> Delete(int key)
        {
            var exam = await GetById(key);

            if (exam == null)
            {
                throw new NoSuchExamExistException($"Exam with ID {key} doesn't exist!");
            }

            _context.Remove(exam);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToDeleteExamException($"Something went wrong while deleting an exam with ID {exam.ExamId}")
                : exam;
        }

        #endregion

        #region Get All Exams
        #region Summary
        /// <summary>
        /// Gets all exams from the database.
        /// </summary>
        /// <returns>Returns all exam objects as an IEnumerable collection.</returns>
        #endregion
        public async Task<IEnumerable<Exam>> GetAll()
        {
            var exams = await _context.Exams
                                       .Include(e => e.Course)
                                       .Include(e => e.Grades)
                                       .ToListAsync();

            return exams.Count == 0
                ? throw new NoExamsExistsException("No exams found in the database!")
                : exams;
        }

        #endregion

        #region Get Exam By Id
        #region Summary
        /// <summary>
        /// Gets a specific exam that matches the given ID.
        /// </summary>
        /// <param name="key">The exam ID.</param>
        /// <returns>Returns the exam object with the matching ID.</returns>
        #endregion
        public async Task<Exam> GetById(int key)
        {
            var exam = await _context.Exams
                                      .Include(e => e.Course)
                                      .Include(e => e.Grades)
                                      .FirstOrDefaultAsync(e => e.ExamId == key);

            return exam == null
                ? throw new NoSuchExamExistException($"Exam with ID {key} doesn't exist!")
                : exam;
        }

        #endregion

        #region Update an Exam

        #region Summary
        /// <summary>
        /// Updates an existing exam in the database.
        /// </summary>
        /// <param name="item">The exam object to be updated.</param>
        /// <returns>Returns the updated exam object.</returns>
        #endregion
        public async Task<Exam> Update(Exam item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var exam = await GetById(item.ExamId);

            if (exam == null)
            {
                throw new NoSuchExamExistException($"Exam with ID {item.ExamId} doesn't exist!");
            }

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return noOfRowsUpdated <= 0
                ? throw new UnableToUpdateExamException($"Something went wrong while updating an exam with ID {exam.ExamId}")
                : exam;
        }

        #endregion

        #endregion
    }

}
