using WebService.Core.Entities.Business;

namespace WebService.Core.Interfaces.IServices
{
    public interface IEmployeeService : IBaseService<EmployeeViewModel>
    {
        Task<EmployeeViewModel> Create(EmployeeCreateViewModel model, CancellationToken cancellationToken);
        Task Update(EmployeeUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<IEnumerable<EmployeeViewModel>> GetAllWithRelation(CancellationToken cancellationToken);
    }
}
