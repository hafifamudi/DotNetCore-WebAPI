using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IMapper;
using WebService.Core.Interfaces.IRepositories;
using WebService.Core.Interfaces.IServices;

namespace WebService.Core.Services
{
    public class EmployeeService : BaseService<Employee, EmployeeViewModel>, IEmployeeService
    {
        private readonly IBaseMapper<Employee, EmployeeViewModel> _EmployeeViewModelMapper;
        private readonly IBaseMapper<EmployeeCreateViewModel, Employee> _EmployeeCreateMapper;
        private readonly IBaseMapper<EmployeeUpdateViewModel, Employee> _EmployeeUpdateMapper;
        private readonly IEmployeeRepository _EmployeeRepository;

        public EmployeeService(
            IBaseMapper<Employee, EmployeeViewModel> EmployeeViewModelMapper,
            IBaseMapper<EmployeeCreateViewModel, Employee> EmployeeCreateMapper,
            IBaseMapper<EmployeeUpdateViewModel, Employee> EmployeeUpdateMapper,
            IEmployeeRepository EmployeeRepository)
            : base(EmployeeViewModelMapper, EmployeeRepository)
        {
            _EmployeeCreateMapper = EmployeeCreateMapper;
            _EmployeeUpdateMapper = EmployeeUpdateMapper;
            _EmployeeViewModelMapper = EmployeeViewModelMapper;
            _EmployeeRepository = EmployeeRepository;
        }

        public async Task<EmployeeViewModel> Create(EmployeeCreateViewModel model, CancellationToken cancellationToken)
        {
            //Mapping through AutoMapper
            var entity = _EmployeeCreateMapper.MapModel(model);
            entity.EntryDate = DateTime.UtcNow;

            return _EmployeeViewModelMapper.MapModel(await _EmployeeRepository.Create(entity, cancellationToken));
        }

        public async Task Update(EmployeeUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _EmployeeRepository.GetById(model.Id, cancellationToken);

            //Mapping through AutoMapper
            _EmployeeUpdateMapper.MapModel(model, existingData);

            // Set additional properties or perform other logic as needed
            existingData.UpdatedDate = DateTime.UtcNow;
            // existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            await _EmployeeRepository.Update(existingData, cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _EmployeeRepository.GetById(id, cancellationToken);
            await _EmployeeRepository.Delete(entity, cancellationToken);
        }

        public Task<IEnumerable<EmployeeViewModel>> GetAllWithRelation(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
