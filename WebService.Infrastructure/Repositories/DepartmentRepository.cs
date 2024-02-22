using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IRepositories;
using WebService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebService.Core.Entities.Business;
using WebService.Core.Exceptions;

namespace WebService.Infrastructure.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Department> BulkCreate(DepartmentCreateViewModel model, CancellationToken cancellationToken)
        {
            // Begin transaction
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Add the department to the database
                var department = new Department
                {
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = model.IsActive
                };

                _dbContext.Departments.Add(department);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // If department insertion is successful, associate the employees and add them to the database
                if (model.Employees != null && model.Employees.Any())
                {
                    var employees = model.Employees.Select(employeeModel => new Employee
                    {
                        EmployeeCode = employeeModel.EmployeeCode,
                        Name = employeeModel.Name,
                        Salary = (double)employeeModel.Salary,
                        DepartmentId = department.Id,
                        Description = employeeModel.Description,
                        IsActive = true,
                        EntryDate = DateTime.UtcNow
                    });

                    _dbContext.Employees.AddRange(employees);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                // Commit transaction
                await transaction.CommitAsync(cancellationToken);

                return department;
            }
            catch (Exception)
            {
                // Rollback transaction if an exception occurs
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<Department> GetAllWithByIdRelation(int id, CancellationToken cancellationToken)
        {
            var department = await _dbContext.Departments
                           .Include(d => d.Employees)
                           .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

            if (department == null)
            {
                throw new NotFoundException("Department not found");
            }

            return department;
        }


        public async Task<IEnumerable<Department>> GetAllWithRelation(CancellationToken cancellationToken)
        {
            var departments = await _dbContext.Departments
                        .Include(d => d.Employees)
                        .ToListAsync(cancellationToken);


            return departments;
        }

    }
}
