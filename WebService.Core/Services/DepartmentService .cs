using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IMapper;
using WebService.Core.Interfaces.IRepositories;
using WebService.Core.Interfaces.IServices;

namespace WebService.Core.Services
{
    public class DepartmentService : BaseService<Department, DepartmentViewModel>, IDepartmentService
    {
        private readonly IBaseMapper<Department, DepartmentViewModel> _DepartmentViewModelMapper;
        private readonly IBaseMapper<DepartmentCreateViewModel, Department> _DepartmentCreateMapper;
        private readonly IBaseMapper<DepartmentUpdateViewModel, Department> _DepartmentUpdateMapper;
        private readonly IDepartmentRepository _DepartmentRepository;

        public DepartmentService(
            IBaseMapper<Department, DepartmentViewModel> DepartmentViewModelMapper,
            IBaseMapper<DepartmentCreateViewModel, Department> DepartmentCreateMapper,
            IBaseMapper<DepartmentUpdateViewModel, Department> DepartmentUpdateMapper,
            IDepartmentRepository DepartmentRepository)
            : base(DepartmentViewModelMapper, DepartmentRepository)
        {
            _DepartmentCreateMapper = DepartmentCreateMapper;
            _DepartmentUpdateMapper = DepartmentUpdateMapper;
            _DepartmentViewModelMapper = DepartmentViewModelMapper;
            _DepartmentRepository = DepartmentRepository;
        }

        public async Task<DepartmentViewModel> Create(DepartmentCreateViewModel model, CancellationToken cancellationToken)
        {
            //Mapping through AutoMapper
            var entity = _DepartmentCreateMapper.MapModel(model);
            entity.EntryDate = DateTime.UtcNow;

            return _DepartmentViewModelMapper.MapModel(await _DepartmentRepository.Create(entity, cancellationToken));
        }

        public async Task Update(DepartmentUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _DepartmentRepository.GetById(model.Id, cancellationToken);

            //Mapping through AutoMapper
            _DepartmentUpdateMapper.MapModel(model, existingData);

            // Set additional properties or perform other logic as needed
            existingData.UpdatedDate = DateTime.UtcNow;
            // existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            await _DepartmentRepository.Update(existingData, cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _DepartmentRepository.GetById(id, cancellationToken);
            await _DepartmentRepository.Delete(entity, cancellationToken);
        }

        public async Task<IEnumerable<DepartmentViewModel>> GetAllWithRelation(CancellationToken cancellationToken)
        {
            return _DepartmentViewModelMapper.MapList(await _DepartmentRepository.GetAllWithRelation(cancellationToken));
        }

        public async Task<DepartmentViewModel> GetAllWithByIdRelation(int id, CancellationToken cancellationToken)
        {
            return _DepartmentViewModelMapper.MapModel(await _DepartmentRepository.GetAllWithByIdRelation(id, cancellationToken));
        }

        public async Task<DepartmentViewModel> BulkCreate(DepartmentCreateViewModel model, CancellationToken cancellationToken)
        {
            return _DepartmentViewModelMapper.MapModel(await _DepartmentRepository.BulkCreate(model, cancellationToken));
        }
    }
}
