using Microsoft.EntityFrameworkCore;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Mappers;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;

namespace StudentManagementApplicationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Controllers
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            #endregion

            #region Swagger
            builder.Services.AddSwaggerGen();
            #endregion

            #region DbContexts

            builder.Services.AddDbContext<StudentManagementContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
                );

            #endregion

            #region Repositories
            builder.Services.AddScoped<IRepository<int, Course>, CourseRepository>();
            builder.Services.AddScoped<IRepository<int, Student>, StudentRepository>();
            builder.Services.AddScoped<IRepository<int, Department>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<int, Exam>, ExamRepository>();
            builder.Services.AddScoped<IRepository<int, Faculty>, FacultyRepository>();
            builder.Services.AddScoped<IRepository<int, Grade>, GradeRepository>();
            builder.Services.AddScoped<IRepository<int, StudentAttendance>, StudentAttendanceRepository>();
            builder.Services.AddScoped<IRepository<int, CourseRegistration>, CourseRegistrationRepository>();
            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Services

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddScoped<IAuthRegisterService<StudentRegisterReturnDTO, StudentRegisterDTO>, StudentAuthService>();
            builder.Services.AddScoped<IAuthLoginService<StudentLoginReturnDTO, StudentLoginDTO>, StudentAuthService>();
            
            builder.Services.AddScoped<IAuthRegisterService<FacultyRegisterReturnDTO, FacultyRegisterDTO>, FacultyAuthService>();
            builder.Services.AddScoped<IAuthLoginService<FacultyLoginReturnDTO, FacultyLoginDTO>, FacultyAuthService>();



            #endregion

            var app = builder.Build();

            #region Swagger Configurations
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #endregion

            #region Pipeline Configurations
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
