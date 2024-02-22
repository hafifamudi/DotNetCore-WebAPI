using WebService.Core.Entities.General;
using WebService.Infrastructure.Data;
using WebService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Project.UnitTest.Infrastructure
{
    public class EmployeeRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private EmployeeRepository _EmployeeRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _EmployeeRepository = new EmployeeRepository(_dbContextMock.Object);
        }

        [Test]
        public async Task AddAsync_ValidEmployee_ReturnsAddedEmployee()
        {

            // Arrange
            var newEmployee = new Employee
            {
                EmployeeCode = "P001",
                Name = "Sample Employee",
                Salary = 9.99f,
                IsActive = true
            };

            var EmployeeDbSetMock = new Mock<DbSet<Employee>>();

            _dbContextMock.Setup(db => db.Set<Employee>())
                          .Returns(EmployeeDbSetMock.Object);

            EmployeeDbSetMock.Setup(dbSet => dbSet.AddAsync(newEmployee, default))
                            .ReturnsAsync((EntityEntry<Employee>)null);

            // Act
            var result = await _EmployeeRepository.Create(newEmployee, It.IsAny<CancellationToken>());


            // Assert
            Assert.NotNull(result);
            Assert.That(result, Is.EqualTo(newEmployee));
        }
    }
}
