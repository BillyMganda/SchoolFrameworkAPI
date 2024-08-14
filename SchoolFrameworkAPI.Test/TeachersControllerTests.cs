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
    public class TeachersControllerTests
    {
        [TestMethod]
        public async Task GetTeacherss_ShouldReturnAllTeachers()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var teachers = new List<GetTeacherResponse>
            {
                new GetTeacherResponse{Id = 1, FirstName="Tony", LastName="Stark", MobileNumber = "0204785421", EmailAddress="tony@example.com", DepartmentId = 1, DateCreated = DateTime.UtcNow},
                new GetTeacherResponse{Id = 2, FirstName="Super", LastName="Man", MobileNumber = "21478541245", EmailAddress="super@example.com", DepartmentId = 2, DateCreated = DateTime.UtcNow}
            }.AsQueryable();

            mockRepository.Setup(repo => repo.GetTeachersAsync())
                .ReturnsAsync(teachers);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.GetTeachersAsync();

            // Assert
            var okResult = result as OkNegotiatedContentResult<IEnumerable<GetTeacherResponse>>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(2, okResult.Content.Count());
            Assert.AreEqual("Tony", okResult.Content.ElementAt(0).FirstName);
            Assert.AreEqual("Super", okResult.Content.ElementAt(1).FirstName);
        }

        [TestMethod]
        public async Task GetTeacher_ShouldReturnTeacher_WhenTeacherExists()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var teacher = new GetTeacherResponse { Id = 1, FirstName = "Tony", LastName = "Stark", MobileNumber = "0204785421", EmailAddress = "tony@example.com", DepartmentId = 1, DateCreated = DateTime.UtcNow };

            mockRepository.Setup(repo => repo.GetTeacherByIdAsync(1))
                          .ReturnsAsync(teacher);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.GetTeacherByIdAsync(1);

            // Assert
            var okResult = result as OkNegotiatedContentResult<GetTeacherResponse>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(1, okResult.Content.Id);
            Assert.AreEqual("Tony", okResult.Content.FirstName);
        }

        [TestMethod]
        public async Task GetTeacher_ShouldReturnNotFound_WhenTeacherDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();

            mockRepository.Setup(repo => repo.GetTeacherByIdAsync(1))
                          .ReturnsAsync((GetTeacherResponse)null);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.GetTeacherByIdAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostTeacher_ShouldReturnCreatedResult_WhenRequestIsValid()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var teacherRequest = new CreateTeacherRequest { FirstName = "Hulk" };

            mockRepository.Setup(repo => repo.CreateTeacherAsync(teacherRequest))
                          .Returns(Task.CompletedTask);

            var controller = new TeachersController(mockRepository.Object);

            // Setup the Url.Link to return a dummy URL
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost/api/teachers/Hulk");
            controller.Url = urlHelper.Object;

            // Act
            var result = await controller.PostTeacherAsync(teacherRequest);

            // Assert
            var createdResult = result as CreatedNegotiatedContentResult<CreateTeacherRequest>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("http://localhost/api/teachers/Hulk", createdResult.Location.ToString());
            Assert.AreEqual("Hulk", createdResult.Content.FirstName);
        }

        [TestMethod]
        public async Task PostTeacher_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.PostTeacherAsync(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Request cannot be null", badRequestResult.Message);
        }

        [TestMethod]
        public async Task PutTeacher_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var teacherRequest = new UpdateTeacherRequest { Id = 1, FirstName = "Updated_Tony" };
            var existingTeacher = new GetTeacherResponse { Id = 1, FirstName = "Tony" };

            mockRepository.Setup(repo => repo.GetTeacherByIdAsync(1))
                          .ReturnsAsync(existingTeacher);
            mockRepository.Setup(repo => repo.UpdateTeacherAsync(teacherRequest))
                          .Returns(Task.CompletedTask);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.PutTeacher(teacherRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PutTeacher_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.PutTeacher(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Request cannot be null", badRequestResult.Message);
        }

        [TestMethod]
        public async Task PutTeacher_ShouldReturnNotFound_WhenTeacherDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var teacherRequest = new UpdateTeacherRequest { Id = 1, FirstName = "Updated_Tony" };

            mockRepository.Setup(repo => repo.GetTeacherByIdAsync(1))
                          .ReturnsAsync((GetTeacherResponse)null);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.PutTeacher(teacherRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteTeacher_ShouldReturnNoContent_WhenTeacherExists()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();
            var existingTeacher = new GetTeacherResponse { Id = 1, FirstName = "Tony" };

            mockRepository.Setup(repo => repo.GetTeacherByIdAsync(1))
                          .ReturnsAsync(existingTeacher);
            mockRepository.Setup(repo => repo.DeleteTeacherAsync(1))
                          .Returns(Task.CompletedTask);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.DeleteTeacher(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteTeacher_ShouldReturnNotFound_WhenTeacherDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ITeacherRepository>();

            mockRepository.Setup(repo => repo.GetTeacherByIdAsync(1))
                          .ReturnsAsync((GetTeacherResponse)null);

            var controller = new TeachersController(mockRepository.Object);

            // Act
            var result = await controller.DeleteTeacher(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
