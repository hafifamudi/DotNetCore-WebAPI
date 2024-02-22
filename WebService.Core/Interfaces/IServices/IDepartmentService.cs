using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;

namespace WebService.Core.Interfaces.IServices
{
    public interface IDepartmentService : IBaseService<DepartmentViewModel>
    {
        Task<DepartmentViewModel> Create(DepartmentCreateViewModel model, CancellationToken cancellationToken);
        Task<DepartmentViewModel> BulkCreate(DepartmentCreateViewModel model, CancellationToken cancellationToken);
        Task Update(DepartmentUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<IEnumerable<DepartmentViewModel>> GetAllWithRelation(CancellationToken cancellationToken);
        Task<DepartmentViewModel> GetAllWithByIdRelation(int id, CancellationToken cancellationToken);
    }
}
