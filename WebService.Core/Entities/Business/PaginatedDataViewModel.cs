
namespace WebService.Core.Entities.Business
{
    public class PaginatedDataViewModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public int TotalCount { get; set; }

        public PaginatedDataViewModel(IEnumerable<T> data, int currentPage, int lastPage, int totalCount)
        {
            Data = data;
            CurrentPage = currentPage;
            LastPage = lastPage;
            TotalCount = totalCount;
        }
    }

}
