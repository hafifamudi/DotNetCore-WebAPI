using System.Linq.Expressions;
using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;

namespace WebService.Core.Interfaces.IRepositories
{
    public interface IDepartmentRepository: IBaseRepository<Department>
    {
        Task<IEnumerable<Department>> GetAllWithRelation(CancellationToken cancellationToken);
        Task<Department> BulkCreate(DepartmentCreateViewModel model, CancellationToken cancellationToken);
        Task<Department> GetAllWithByIdRelation(int id, CancellationToken cancellationToken);
    }
}