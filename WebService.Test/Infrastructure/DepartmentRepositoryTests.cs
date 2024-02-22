using WebService.Core.Entities.General;
using WebService.Infrastructure.Data;
using WebService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Project.UnitTest.Infrastructure
{
    public class DepartmentRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private DepartmentRepository _DepartmentRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _DepartmentRepository = new DepartmentRepository(_dbContextMock.Object);
        }

        [Test]
        public async Task AddAsync_ValidDepartment_ReturnsAddedDepartment()
        {

            // Arrange
            var newDepartment = new Department
            {
                Name = "Sample Department",
                Description = "Sample Desc",
                IsActive = true
            };

            var DepartmentDbSetMock = new Mock<DbSet<Department>>();

            _dbContextMock.Setup(db => db.Set<Department>())
                          .Returns(DepartmentDbSetMock.Object);

            DepartmentDbSetMock.Setup(dbSet => dbSet.AddAsync(newDepartment, default))
                            .ReturnsAsync((EntityEntry<Department>)null);

            // Act
            var result = await _DepartmentRepository.Create(newDepartment, It.IsAny<CancellationToken>());


            // Assert
            Assert.NotNull(result);
            Assert.That(result, Is.EqualTo(newDepartment));
        }
    }
}
