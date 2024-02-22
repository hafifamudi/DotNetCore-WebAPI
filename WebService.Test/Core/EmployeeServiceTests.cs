using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IMapper;
using WebService.Core.Interfaces.IRepositories;
using WebService.Core.Services;
using Moq;

namespace Project.UnitTest
{
    public class EmployeeServiceTests
    {
        private Mock<IBaseMapper<Employee, EmployeeViewModel>> _EmployeeViewModelMapperMock;
        private Mock<IBaseMapper<EmployeeCreateViewModel, Employee>> _EmployeeCreateMapperMock;
        private Mock<IBaseMapper<EmployeeUpdateViewModel, Employee>> _EmployeeUpdateMapperMock;
        private Mock<IEmployeeRepository> _EmployeeRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _EmployeeViewModelMapperMock = new Mock<IBaseMapper<Employee, EmployeeViewModel>>();
            _EmployeeCreateMapperMock = new Mock<IBaseMapper<EmployeeCreateViewModel, Employee>>();
            _EmployeeUpdateMapperMock = new Mock<IBaseMapper<EmployeeUpdateViewModel, Employee>>();
            _EmployeeRepositoryMock = new Mock<IEmployeeRepository>();
        }

        [Test]
        public async Task CreateEmployeeAsync_ValidEmployee_ReturnsCreatedEmployeeViewModel()
        {
            // Arrange
            var EmployeeService = new EmployeeService(
                _EmployeeViewModelMapperMock.Object,
                _EmployeeCreateMapperMock.Object,
                _EmployeeUpdateMapperMock.Object,
                _EmployeeRepositoryMock.Object);

            var newEmployeeCreateViewModel = new EmployeeCreateViewModel
            {
                EmployeeCode = "P001",
                Name = "Sample Employee",
                Salary = 9.99f,
                Description = "Sample description",
                IsActive = true
            };

            var newEmployeeViewModel = new EmployeeViewModel
            {
                EmployeeCode = "P001",
                Name = "Sample Employee",
                Salary = 9.99f,
                Description = "Sample description",
                IsActive = true
            };

            var createdEmployee = new Employee
            {
                EmployeeCode = "P001",
                Name = "Sample Employee",
                Salary = 9.99f,
                Description = "Sample description",
                IsActive = true
            };

            _EmployeeCreateMapperMock.Setup(mapper => mapper.MapModel(newEmployeeCreateViewModel))
                              .Returns(createdEmployee);

            _EmployeeRepositoryMock.Setup(repo => repo.Create(createdEmployee, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(createdEmployee);

            _EmployeeViewModelMapperMock.Setup(mapper => mapper.MapModel(createdEmployee))
                                       .Returns(newEmployeeViewModel);

            // Act
            var result = await EmployeeService.Create(newEmployeeCreateViewModel, It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(result);
            Assert.That(result.EmployeeCode, Is.EqualTo(newEmployeeViewModel.EmployeeCode));
        }
    }

}