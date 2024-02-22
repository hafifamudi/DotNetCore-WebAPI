using WebService.API.Controllers.V1;
using WebService.Core.Entities.Business;
using WebService.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace WebService.UnitTest.API
{
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeService> _EmployeeServiceMock;
        private Mock<ILogger<EmployeeController>> _loggerMock;
        private EmployeeController _EmployeeController;

        [SetUp]
        public void Setup()
        {
            _EmployeeServiceMock = new Mock<IEmployeeService>();
            _loggerMock = new Mock<ILogger<EmployeeController>>();
            _EmployeeController = new EmployeeController(_loggerMock.Object, _EmployeeServiceMock.Object);
        }

        [Test]
        public async Task Get_ReturnsViewWithListOfEmployees()
        {
            // Arrange
            var Employees = new List<EmployeeViewModel>
            {
                new EmployeeViewModel { Id = 1, EmployeeCode = "P001", Description = "Employee A", Salary = 9.99f, IsActive = true },
                new EmployeeViewModel { Id = 1, EmployeeCode = "P002", Description = "Employee B", Salary = 5.99f, IsActive = true },
            };

            _EmployeeServiceMock.Setup(service => service.GetAll(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Employees);

            // Act
            var result = await _EmployeeController.Get(It.IsAny<CancellationToken>());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);

            var model = ((ResponseViewModel<IEnumerable<EmployeeViewModel>>)okObjectResult.Value).Data;
            Assert.NotNull(model);
            Assert.That(model.Count(), Is.EqualTo(Employees.Count));

        }

        // Add more test methods for other controller actions, such as Create, Update, Delete, etc.
        [Test]
        public async Task Get_WithValidId_ReturnsOk()
        {
            // Arrange
            var employeeId = 1;
            var employee = new EmployeeViewModel { Id = employeeId, EmployeeCode = "P001", Description = "Employee A", Salary = 9.99f, IsActive = true };

            _EmployeeServiceMock.Setup(service => service.GetById(employeeId, CancellationToken.None))
                                .ReturnsAsync(employee);

            // Act
            var result = await _EmployeeController.Get(employeeId, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);

            var model = (ResponseViewModel<EmployeeViewModel>)okObjectResult.Value;
            Assert.NotNull(model);
            Assert.IsTrue(model.Success);
            Assert.That(model.Message, Is.EqualTo("Employee retrieved successfully"));
            Assert.That(model.Data, Is.EqualTo(employee));
        }
    }
}
