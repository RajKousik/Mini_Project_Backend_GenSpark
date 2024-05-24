using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Repositories;

namespace StudentManagementApplicationAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<int, Student> _studentRepo;
        private readonly IRepository<int, Department> _departmentRepo;
        private readonly IMapper _mapper;

        public StudentService(IRepository<int, Student> studentRepo, IMapper mapper, IRepository<int, Department> departmentRepo)
        {
            _studentRepo = studentRepo;
            _mapper = mapper;
            _departmentRepo = departmentRepo;
        }

        public async Task<StudentDTO> DeleteStudent(string email)
        {
            try
            {
                var student = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
                }

                await _studentRepo.Delete(student.StudentRollNo);
                return _mapper.Map<StudentDTO>(student);
            }
            catch (UnableToDeleteStudentException ex)
            {
                throw new UnableToDeleteStudentException(ex.Message);
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<StudentDTO>> GetStudentByName(string name)
        {
            try
            {
                var students = (await _studentRepo.GetAll()).Where(f => f.Name.ToLower().Contains(name.ToLower())).ToList();
                if (students.Count == 0)
                {
                    throw new NoStudentsExistsException();
                }
                return _mapper.Map<IEnumerable<StudentDTO>>(students);
            }
            catch (NoStudentsExistsException ex)
            {
                throw new NoStudentsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudents()
        {
            try
            {
                var students = (await _studentRepo.GetAll()).ToList();
                if (students.Count == 0)
                {
                    throw new NoStudentsExistsException();
                }
                return _mapper.Map<IEnumerable<StudentDTO>>(students);
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentDTO> GetStudentByEmail(string email)
        {
            try
            {
                var student = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
                }

                return _mapper.Map<StudentDTO>(student);
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentDTO> GetStudentById(int studentRollNo)
        {
            try
            {
                var student = await _studentRepo.GetById(studentRollNo);
                if (student == null)
                {
                    throw new NoSuchStudentExistException($"Student with roll number {studentRollNo} does not exist.");
                }

                return _mapper.Map<StudentDTO>(student);
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StudentDTO>> GetStudentsByDepartment(int departmentId)
        {
            try
            {
                var department = await _departmentRepo.GetById(departmentId);
                if (department == null)
                {
                    throw new NoSuchDepartmentExistException($"Department with id {departmentId} does not exist.");
                }

                var students = (await _studentRepo.GetAll()).Where(s => s.DepartmentId == departmentId).ToList();
                if (students.Count == 0)
                {
                    throw new NoStudentsExistsException($"No students in the department {departmentId}");
                }
                return _mapper.Map<IEnumerable<StudentDTO>>(students);
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentDTO> UpdateStudent(StudentDTO dto, string email)
        {
            try
            {
                var studentInDB = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
                if (studentInDB == null)
                {
                    throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
                }

                if (!string.IsNullOrEmpty(dto.Name))
                {
                    studentInDB.Name = dto.Name;
                }

                if (dto.DOB != null)
                {
                    studentInDB.DOB = dto.DOB;
                }

                if (!string.IsNullOrEmpty(dto.Gender))
                {
                    studentInDB.Gender = dto.Gender;
                }

                if (!string.IsNullOrEmpty(dto.Mobile))
                {
                    studentInDB.Mobile = dto.Mobile;
                }

                if (!string.IsNullOrEmpty(dto.Address))
                {
                    studentInDB.Address = dto.Address;
                }

                var departmentExists = await _departmentRepo.GetById(dto.DepartmentId);
                if (departmentExists == null)
                {
                    throw new NoSuchDepartmentExistException();
                }
                if (departmentExists.Name.ToLower().Contains("Admin".ToLower()))
                {
                    throw new CannotAddStudentToAdminDepartmentException();
                }
                studentInDB.DepartmentId = dto.DepartmentId;
                await _studentRepo.Update(studentInDB);
                return _mapper.Map<StudentDTO>(studentInDB);
            }
            catch (NoSuchStudentExistException ex)
            {
                throw new NoSuchStudentExistException(ex.Message);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (CannotAddStudentToAdminDepartmentException ex)
            {
                throw new CannotAddStudentToAdminDepartmentException(ex.Message);
            }
            catch (UnableToUpdateStudentException ex)
            {
                throw new UnableToUpdateStudentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
