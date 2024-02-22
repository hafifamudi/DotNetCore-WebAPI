using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;

namespace WebService.Core.Interfaces.IRepositories
{
    public interface IEmployeeRepository: IBaseRepository<Employee>
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllWithRelation(CancellationToken cancellationToken);
    }
}