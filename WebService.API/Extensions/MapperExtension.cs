using AutoMapper;
using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IMapper;
using WebService.Core.Mapper;

namespace WebService.API.Extensions
{
    public static class MapperExtension
    {
        public static IServiceCollection RegisterMapperService(this IServiceCollection services)
        {

            #region Mapper

            services.AddSingleton<IMapper>(sp => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeViewModel>();
                cfg.CreateMap<EmployeeCreateViewModel, Employee>();
                cfg.CreateMap<EmployeeUpdateViewModel, Employee>();

                cfg.CreateMap<Department, DepartmentViewModel>();
                cfg.CreateMap<DepartmentCreateViewModel, Department>();
                cfg.CreateMap<DepartmentUpdateViewModel, Department>();
            }).CreateMapper());

            // Register the IMapperService implementation with your dependency injection container
            services.AddSingleton<IBaseMapper<Employee, EmployeeViewModel>, BaseMapper<Employee, EmployeeViewModel>>();
            services.AddSingleton<IBaseMapper<EmployeeCreateViewModel, Employee>, BaseMapper<EmployeeCreateViewModel, Employee>>();
            services.AddSingleton<IBaseMapper<EmployeeUpdateViewModel, Employee>, BaseMapper<EmployeeUpdateViewModel, Employee>>();

            services.AddSingleton<IBaseMapper<Department, DepartmentViewModel>, BaseMapper<Department, DepartmentViewModel>>();
            services.AddSingleton<IBaseMapper<DepartmentCreateViewModel, Department>, BaseMapper<DepartmentCreateViewModel, Department>>();
            services.AddSingleton<IBaseMapper<DepartmentUpdateViewModel, Department>, BaseMapper<DepartmentUpdateViewModel, Department>>();
            #endregion

            return services;
        }
    }
}
