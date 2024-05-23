using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;

namespace StudentManagementApplicationAPI.Repositories
{
    public class StudentRepository : IRepository<int, Student>
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
        public StudentRepository(StudentManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        #region Adds a Student
        #region Summary
        /// <summary>
        /// Adds a new student in the database.
        /// </summary>
        /// <param name="item">The student object to be added.</param>
        /// <returns>Returns the newly added student object.</returns>
        #endregion
        public async Task<Student> Add(Student item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            
            _context.Add(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            if(noOfRowsUpdated <= 0)
                throw new UnableToAddStudentException($"Something went wrong while adding a student with Roll No {item.StudentRollNo}");
            
            return item;
        }
        #endregion

        #region Deletes a Student
        #region Summary
        /// <summary>
        /// Delete a existing student in the database.
        /// </summary>
        /// <param name="key">The student primary key that's to be deleted.</param>
        /// <returns>Returns the deleted student object.</returns>
        #endregion
        public async Task<Student> Delete(int key)
        {
            var student = await GetById(key);

            if (student == null)
                throw new NoSuchStudentExistException($"Student with given Roll No {key} doesn't exist!"); 

            _context.Remove(student);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToDeleteStudentException($"Something went wrong while deleting a student with Roll No {student.StudentRollNo}")
                    :
                    student;

        }
        #endregion

        #region Get all Students
        #region Summary
        /// <summary>
        /// To get all the students in the database.
        /// </summary>
        /// <returns>Returns all the students in the database.</returns>
        #endregion
        public async Task<IEnumerable<Student>> GetAll()
        {
            var students = await _context.Students.Include(s => s.Department).ToListAsync();

            return (students.Count == 0)
                    ?
                    throw new NoStudentsExistsException("No Students found in the database!")
                    :
                    students;
        }
        #endregion

        #region Get a Specific Student By Id
        #region Summary
        /// <summary>
        /// To get a specific student which matches the given roll number.
        /// </summary>
        /// <param name="key">The student roll No</param>
        /// <returns>Returns the student object which has matching rollNo.</returns>
        #endregion
        public async Task<Student> GetById(int key)
        {
            var student = await _context.Students.Include(s => s.Department)
                                                .FirstOrDefaultAsync(s => s.StudentRollNo == key);

            return student == null 
                    ? 
                    throw new NoSuchStudentExistException($"Student with given Roll No {key} doesn't exist!") 
                    : 
                    student;
        }
        #endregion

        #region Update a Student
        #region Summary
        /// <summary>
        /// Update a existing student in the database.
        /// </summary>
        /// <param name="item">The student object to be updated.</param>
        /// <returns>Returns the updated student object.</returns>
        #endregion
        public async Task<Student> Update(Student item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            

            var student = await GetById(item.StudentRollNo);

            if (student == null)
                throw new NoSuchStudentExistException($"Student with given Roll No {item.StudentRollNo} doesn't exist!");

            _context.Update(item);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToUpdateStudentException($"Something went wrong while updating a student with Roll No {student.StudentRollNo}")
                    :
                    student;

        }
        #endregion

        #endregion
    }
}
