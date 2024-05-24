using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.StudentExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;

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
            var student = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
            if (student == null)
            {
                throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
            }

            await _studentRepo.Delete(student.StudentRollNo);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudents()
        {
            var students = (await _studentRepo.GetAll()).ToList();
            if(students.Count == 0)
            {
                throw new NoStudentsExistsException();
            }
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO> GetStudentByEmail(string email)
        {
            var student = (await _studentRepo.GetAll()).FirstOrDefault(s => s.Email == email);
            if (student == null)
            {
                throw new NoSuchStudentExistException($"Student with email {email} does not exist.");
            }

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> GetStudentById(int studentRollNo)
        {
            var student = await _studentRepo.GetById(studentRollNo);
            if (student == null)
            {
                throw new NoSuchStudentExistException($"Student with roll number {studentRollNo} does not exist.");
            }

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<IEnumerable<StudentDTO>> GetStudentsByDepartment(int departmentId)
        {
            var department = await _departmentRepo.GetById(departmentId);
            if (department == null)
            {
                throw new NoSuchDepartmentExistException($"Department with id {departmentId} does not exist.");
            }

            var students = (await _studentRepo.GetAll()).Where(s => s.DepartmentId == departmentId).ToList();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO> UpdateStudent(StudentDTO dto, string email)
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

            var departmentExists = _departmentRepo.GetById(dto.DepartmentId);
            if (departmentExists != null && dto.DepartmentId == 2)
            {
                studentInDB.DepartmentId = dto.DepartmentId;
            }
            else if(dto.DepartmentId == 2)
            {
                throw new CannotAddStudentToAdminDepartmentException();
            }
            else
            {
                throw new NoSuchDepartmentExistException();
            }



            await _studentRepo.Update(studentInDB);
            return _mapper.Map<StudentDTO>(studentInDB);
        }
    }

}
