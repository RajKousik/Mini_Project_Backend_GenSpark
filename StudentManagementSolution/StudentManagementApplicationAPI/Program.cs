using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentManagementApplicationAPI.Contexts;
using StudentManagementApplicationAPI.Interfaces;
using StudentManagementApplicationAPI.Mappers;
using StudentManagementApplicationAPI.Models.Db_Models;
using StudentManagementApplicationAPI.Models.DTOs.FacultyDTOs;
using StudentManagementApplicationAPI.Models.DTOs.StudentDTOs;
using StudentManagementApplicationAPI.Repositories;
using StudentManagementApplicationAPI.Services;
using System.Text;

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

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                           new OpenApiSecurityScheme
                           {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                           },
                           new string[] {}
                     }
                });
            });

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
            
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IStudentService , StudentService>();
            builder.Services.AddScoped<IFacultyService , FacultyService>();

            builder.Services.AddScoped<IDepartmentService , DepartmentService>();
            builder.Services.AddScoped<ICourseService , CourseService>();
            builder.Services.AddScoped<ICourseRegistrationService , CourseRegistrationService>();



            #endregion

            #region Authentication

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                    };

                });

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

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
