using WebService.Core.Entities.Business;
using WebService.Core.Entities.General;
using WebService.Core.Interfaces.IMapper;
using WebService.Core.Interfaces.IRepositories;
using WebService.Core.Interfaces.IServices;
using WebService.Core.Services;
using Moq;

namespace Project.UnitTest
{
    public class DepartmentServiceTests
    {
        private Mock<IBaseMapper<Department, DepartmentViewModel>> _DepartmentViewModelMapperMock;
        private Mock<IBaseMapper<DepartmentCreateViewModel, Department>> _DepartmentCreateMapperMock;
        private Mock<IBaseMapper<DepartmentUpdateViewModel, Department>> _DepartmentUpdateMapperMock;
        private Mock<IDepartmentRepository> _DepartmentRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _DepartmentViewModelMapperMock = new Mock<IBaseMapper<Department, DepartmentViewModel>>();
            _DepartmentCreateMapperMock = new Mock<IBaseMapper<DepartmentCreateViewModel, Department>>();
            _DepartmentUpdateMapperMock = new Mock<IBaseMapper<DepartmentUpdateViewModel, Department>>();
            _DepartmentRepositoryMock = new Mock<IDepartmentRepository>();
        }

        [Test]
        public async Task CreateDepartmentAsync_ValidDepartment_ReturnsCreatedDepartmentViewModel()
        {
            // Arrange
            var Department = new DepartmentService(
                _DepartmentViewModelMapperMock.Object,
                _DepartmentCreateMapperMock.Object,
                _DepartmentUpdateMapperMock.Object,
                _DepartmentRepositoryMock.Object);

            var newDepartmentCreateViewModel = new DepartmentCreateViewModel
            {
                Name = "Sample Department",
                Description = "Sample description",
                IsActive = true
            };

            var newDepartmentViewModel = new DepartmentViewModel
            {
                Name = "Sample Department",
                Description = "Sample description",
                IsActive = true
            };

            var createdDepartment = new Department
            {
                Name = "Sample Department",
                Description = "Sample description",
                IsActive = true
            };

            _DepartmentCreateMapperMock.Setup(mapper => mapper.MapModel(newDepartmentCreateViewModel))
                              .Returns(createdDepartment);

            _DepartmentRepositoryMock.Setup(repo => repo.Create(createdDepartment, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(createdDepartment);

            _DepartmentViewModelMapperMock.Setup(mapper => mapper.MapModel(createdDepartment))
                                       .Returns(newDepartmentViewModel);

            // Act
            var result = await Department.Create(newDepartmentCreateViewModel, It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(newDepartmentViewModel.Name));
        }
    }

}