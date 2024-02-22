using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IRepositories;
using WebService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebService.Core.Entities.Business;
using System.Threading;
using System.Linq.Expressions;

namespace WebService.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllWithRelation(CancellationToken cancellationToken = default)
        {
          var employees = await _dbContext.Employees
            .Include(e => e.Department)
            .ToListAsync(cancellationToken);


            return (IEnumerable<EmployeeViewModel>)employees;
        }

    }
}
