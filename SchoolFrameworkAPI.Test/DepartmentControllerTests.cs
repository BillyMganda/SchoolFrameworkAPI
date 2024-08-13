using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolFrameworkAPI.Controllers;
using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace SchoolFrameworkAPI.Test
{
    [TestClass]
    public class DepartmentControllerTests
    {        
        [TestMethod]
        public async Task GetDepartments_ShouldReturnAllDepartments()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR", DateCreated = DateTime.UtcNow },
                new Department { Id = 2, Name = "IT", DateCreated = DateTime.UtcNow }
            }.AsQueryable();

            mockRepository.Setup(repo => repo.GetDepartmentsAsync())
                          .ReturnsAsync(departments);

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.GetDepartments();

            // Assert
            var okResult = result as OkNegotiatedContentResult<IEnumerable<Department>>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(2, okResult.Content.Count());
            Assert.AreEqual("HR", okResult.Content.ElementAt(0).Name);
            Assert.AreEqual("IT", okResult.Content.ElementAt(1).Name);
        }

        [TestMethod]
        public async Task GetDepartment_ShouldReturnDepartment_WhenDepartmentExists()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var department = new Department { Id = 1, Name = "HR" };

            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync(department);

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.GetDepartment(1);

            // Assert
            var okResult = result as OkNegotiatedContentResult<Department>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(1, okResult.Content.Id);
            Assert.AreEqual("HR", okResult.Content.Name);
        }

        [TestMethod]
        public async Task GetDepartment_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();

            // Setup repository to return null when the department is not found
            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync((Department)null);

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.GetDepartment(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostDepartment_ShouldReturnCreatedResult_WhenRequestIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var departmentRequest = new CreateDepartmentRequest { Name = "Finance" };

            mockRepository.Setup(repo => repo.CreateDepartmentAsync(departmentRequest))
                          .Returns(Task.CompletedTask); // Simulate async method

            var controller = new DepartmentsController(mockRepository.Object);

            // Setup the Url.Link to return a dummy URL
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost/api/departments/Finance");
            controller.Url = urlHelper.Object;

            // Act
            var result = await controller.PostDepartment(departmentRequest);

            // Assert
            var createdResult = result as CreatedNegotiatedContentResult<CreateDepartmentRequest>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("http://localhost/api/departments/Finance", createdResult.Location.ToString());
            Assert.AreEqual("Finance", createdResult.Content.Name);
        }

        [TestMethod]
        public async Task PostDepartment_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.PostDepartment(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Department cannot be null", badRequestResult.Message);
        }

        [TestMethod]
        public async Task PutDepartment_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var departmentRequest = new UpdateDepartmentRequest { Id = 1, Name = "Updated Department" };
            var existingDepartment = new Department { Id = 1, Name = "Old Department" };

            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync(existingDepartment);
            mockRepository.Setup(repo => repo.UpdateDepartmentAsync(departmentRequest))
                          .Returns(Task.CompletedTask); // Simulate async method

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.PutDepartment(departmentRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PutDepartment_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.PutDepartment(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Department cannot be null", badRequestResult.Message);
        }

        [TestMethod]
        public async Task PutDepartment_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var departmentRequest = new UpdateDepartmentRequest { Id = 1, Name = "Updated Department" };

            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync((Department)null); // Simulate department not found

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.PutDepartment(departmentRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteDepartment_ShouldReturnNoContent_WhenDepartmentExists()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();
            var existingDepartment = new Department { Id = 1, Name = "HR" };

            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync(existingDepartment);
            mockRepository.Setup(repo => repo.DeleteDepartmentAsync(1))
                          .Returns(Task.CompletedTask); // Simulate async method

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.DeleteDepartment(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteDepartment_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();

            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync((Department)null); // Simulate department not found

            var controller = new DepartmentsController(mockRepository.Object);

            // Act
            var result = await controller.DeleteDepartment(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
