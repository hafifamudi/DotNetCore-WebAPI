using WebService.Core.Interfaces.IRepositories;
using WebService.Core.Interfaces.IServices;
using WebService.Core.Services;
using WebService.Infrastructure.Repositories;

namespace WebService.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            #endregion

            #region Repositories
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            #endregion

            return services;
        }
    }
}
