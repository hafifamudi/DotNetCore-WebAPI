using WebService.API.Controllers.V1;
using WebService.Core.Entities.Business;
using WebService.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace WebService.UnitTest.API
{
    public class DepartmentControllerTests
    {
        private Mock<IDepartmentService> _DepartmentServiceMock;
        private Mock<ILogger<DepartmentController>> _loggerMock;
        private DepartmentController _DepartmentController;

        [SetUp]
        public void Setup()
        {
            _DepartmentServiceMock = new Mock<IDepartmentService>();
            _loggerMock = new Mock<ILogger<DepartmentController>>();
            _DepartmentController = new DepartmentController(_loggerMock.Object, _DepartmentServiceMock.Object);
        }

        [Test]
        public async Task Get_ReturnsViewWithListOfDepartments()
        {
            // Arrange
            var Departments = new List<DepartmentViewModel>
            {
                new DepartmentViewModel { Id = 1, Name = "IT", Description = "Department A", IsActive = true },
                new DepartmentViewModel { Id = 1, Name = "Marketing", Description = "Department B", IsActive = true },
            };

            _DepartmentServiceMock.Setup(service => service.GetAll(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Departments);

            // Act
            var result = await _DepartmentController.Get(It.IsAny<CancellationToken>());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);

            var model = ((ResponseViewModel<IEnumerable<DepartmentViewModel>>)okObjectResult.Value).Data;
            Assert.NotNull(model);
            Assert.That(model.Count(), Is.EqualTo(Departments.Count));

        }
    }
}
