﻿using AutoMapper;
using StudentManagementApplicationAPI.Exceptions.DepartmentExceptions;
using StudentManagementApplicationAPI.Exceptions.FacultyExceptions;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.DepartmentDTOs;
using StudentManagementApplicationAPI.Models.Enums;

namespace StudentManagementApplicationAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        #region Private Fields

        private readonly IRepository<int, Department> _departmentRepository;
        private readonly IRepository<int, Faculty> _facultyRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public DepartmentService(IRepository<int, Department> departmentRepository, IMapper mapper,
            IRepository<int, Faculty> facultyRepository)
        {
            _departmentRepository = departmentRepository;
            _facultyRepository = facultyRepository;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new department.
        /// </summary>
        /// <param name="departmentDTO">The department data transfer object.</param>
        /// <returns>The added department data transfer object.</returns>
        public async Task<DepartmentReturnDTO> AddDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                var departmentExist = (await _departmentRepository.GetAll()).FirstOrDefault(d => d.Name == departmentDTO.Name);

                if(departmentExist != null)
                {
                    throw new DepartmentAlreadyExistException();
                }

                var isFacultyExist = await _facultyRepository.GetById((int)departmentDTO.HeadId);

                if (isFacultyExist == null)
                {
                    throw new NoSuchFacultyExistException();
                }

                if(isFacultyExist.Role == RoleType.Admin || isFacultyExist.Role == RoleType.Head_Of_Department)
                {
                    throw new UnableToAddDepartmentException("Unable to add department head");
                }

                

                

                var department = _mapper.Map<Department>(departmentDTO);

                var addedDepartment = await _departmentRepository.Add(department);

                if(addedDepartment == null)
                {
                    throw new UnableToAddDepartmentException();
                }

                isFacultyExist.DepartmentId = addedDepartment.DeptId;
                isFacultyExist.Role = RoleType.Head_Of_Department;

                await _facultyRepository.Update(isFacultyExist);

                var returnDTO = _mapper.Map<DepartmentReturnDTO>(addedDepartment);
                returnDTO.DeptId = addedDepartment.DeptId;
                return returnDTO;
            }
            catch (DepartmentAlreadyExistException ex)
            {
                throw new DepartmentAlreadyExistException(ex.Message);
            }
            catch (NoSuchFacultyExistException ex)
            {
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (UnableToAddDepartmentException ex)
            {
                throw new UnableToAddDepartmentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Deletes a department.
        /// </summary>
        /// <param name="departmentId">The ID of the department to delete.</param>
        /// <returns>The deleted department data transfer object.</returns>
        public async Task<DepartmentDTO> DeleteDepartment(int departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetById(departmentId);
                if (department == null)
                {
                    throw new NoDepartmentsExistsException();
                }

                var deletedDepartment = await _departmentRepository.Delete(departmentId);
                if(deletedDepartment == null)
                {
                    throw new UnableToDeleteDepartmentException();
                }
                return _mapper.Map<DepartmentDTO>(department);
            }
            catch (UnableToDeleteDepartmentException ex)
            {
                throw new UnableToDeleteDepartmentException(ex.Message);
            }
            catch (NoDepartmentsExistsException ex)
            {
                throw new NoDepartmentsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a department by its ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>The department data transfer object.</returns>
        public async Task<DepartmentDTO> GetDepartmentById(int departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetById(departmentId);
                if (department == null)
                {
                    throw new NoSuchDepartmentExistException();
                }

                return _mapper.Map<DepartmentDTO>(department);
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

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>The list of all department data transfer objects.</returns>
        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartments()
        {
            try
            {
                var departments = (await _departmentRepository.GetAll()).ToList();
                if(departments.Count == 0)
                {
                    throw new NoDepartmentsExistsException();
                }
                return _mapper.Map<IEnumerable<DepartmentDTO>>(departments); 
            }
            catch (NoDepartmentsExistsException ex)
            {
                throw new NoDepartmentsExistsException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve departments", ex);
            }
        }

        /// <summary>
        /// Changes the head of a department.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <param name="newHeadDepartmentId">The ID of the new head of the department.</param>
        /// <returns>The updated department data transfer object.</returns>
        public async Task<DepartmentDTO> ChangeDepartmentHead(int departmentId, int newHeadDepartmentId)
        {
            try
            {
                var department = await _departmentRepository.GetById(departmentId);
                if (department == null)
                {
                    throw new NoSuchDepartmentExistException();
                }

                

                var isFacultyExist = await _facultyRepository.GetById(newHeadDepartmentId);

                if (isFacultyExist == null)
                {
                    throw new NoSuchFacultyExistException();
                }

                if(isFacultyExist.Role == RoleType.Admin)
                {
                    throw new Exception("Admin cannot be added as a department Head");
                }

                if(isFacultyExist.DepartmentId != departmentId)
                {
                    throw new UnableToUpdateDepartmentException("Faculty from other department cannot be made as Head");
                }

                var currentDeptHead = await _facultyRepository.GetById((int)department.HeadId);
                currentDeptHead.Role = RoleType.Proffesors;

                isFacultyExist.Role = RoleType.Head_Of_Department;
                department.HeadId = newHeadDepartmentId;


                await _facultyRepository.Update(currentDeptHead);
                await _facultyRepository.Update(isFacultyExist);
                await _departmentRepository.Update(department);

                return _mapper.Map<DepartmentDTO>(department);
            }
            catch (NoSuchDepartmentExistException ex)
            {
                throw new NoSuchDepartmentExistException(ex.Message);
            }
            catch (NoSuchFacultyExistException ex)
            {
                throw new NoSuchFacultyExistException(ex.Message);
            }
            catch (UnableToUpdateDepartmentException ex)
            {
                throw new UnableToUpdateDepartmentException(ex.Message);
            }
            catch (UnableToUpdateFacultyException ex)
            {
                throw new UnableToUpdateFacultyException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }

}