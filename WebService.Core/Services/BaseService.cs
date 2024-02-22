﻿using System.Linq.Expressions;
using WebService.Core.Common;
using WebService.Core.Entities.Business;
using WebService.Core.Interfaces.IMapper;
using WebService.Core.Interfaces.IRepositories;
using WebService.Core.Interfaces.IServices;

namespace WebService.Core.Services
{
    public class BaseService<T, TViewModel> : IBaseService<TViewModel>
        where T : class
        where TViewModel : class
    {
        private readonly IBaseMapper<T, TViewModel> _viewModelMapper;
        private readonly IBaseRepository<T> _repository;

        public BaseService(
            IBaseMapper<T, TViewModel> viewModelMapper,
            IBaseRepository<T> repository)
        {
            _viewModelMapper = viewModelMapper;
            _repository = repository;
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAll(CancellationToken cancellationToken)
        {
            return _viewModelMapper.MapList(await _repository.GetAll(cancellationToken));
        }

        public virtual async Task<PaginatedDataViewModel<TViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var paginatedData = await _repository.GetPaginatedData(pageNumber, pageSize, cancellationToken);
            var mappedData = _viewModelMapper.MapList(paginatedData.Data);
            var paginatedDataViewModel = new PaginatedDataViewModel<TViewModel>(mappedData.ToList(), paginatedData.CurrentPage, paginatedData.LastPage, paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public virtual async Task<PaginatedDataViewModel<TViewModel>> GetPaginatedDataWithFilter(int pageNumber, int pageSize, List<ExpressionFilter> filters, CancellationToken cancellationToken)
        {
            var paginatedData = await _repository.GetPaginatedDataWithFilter(pageNumber, pageSize, filters, cancellationToken);
            var mappedData = _viewModelMapper.MapList(paginatedData.Data);
            var paginatedDataViewModel = new PaginatedDataViewModel<TViewModel>(mappedData.ToList(), paginatedData.CurrentPage, paginatedData.LastPage, paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public virtual async Task<TViewModel> GetById<Tid>(Tid id, CancellationToken cancellationToken)
        {
            return _viewModelMapper.MapModel(await _repository.GetById(id, cancellationToken));
        }

        public virtual async Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken)
        {
            return await _repository.IsExists(key, value?.ToString(), cancellationToken);
        }

        public virtual async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, CancellationToken cancellationToken)
        {
            return await _repository.IsExistsForUpdate(id, key, value, cancellationToken);
        }
    }

}
