using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolFrameworkAPI.Controllers;
using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using System.Web.Http;

namespace SchoolFrameworkAPI.Test
{
    [TestClass]
    public class FormsControllerTests
    {
        [TestMethod]
        public async Task GetForms_ShouldReturnAllForms()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var forms = new List<GetFormResponse>
            {
                new GetFormResponse{Id = 1, Name="Form1", DateCreated = DateTime.UtcNow},
                new GetFormResponse{Id = 2, Name="Form2", DateCreated = DateTime.UtcNow},
            }.AsQueryable();

            mockRepository.Setup(repo => repo.GetFormsAsync())
                .ReturnsAsync(forms);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.GetForms();

            // Assert
            var okResult = result as OkNegotiatedContentResult<IEnumerable<GetFormResponse>>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(2, okResult.Content.Count());
            Assert.AreEqual("Form1", okResult.Content.ElementAt(0).Name);
            Assert.AreEqual("Form2", okResult.Content.ElementAt(1).Name);
        }

        [TestMethod]
        public async Task GetForm_ShouldReturnForm_WhenFormExists()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var form = new GetFormResponse { Id = 1, Name = "Form1", DateCreated = DateTime.UtcNow };

            mockRepository.Setup(repo => repo.GetFormByIdAsync(1))
                          .ReturnsAsync(form);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.GetForm(1);

            // Assert
            var okResult = result as OkNegotiatedContentResult<GetFormResponse>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(1, okResult.Content.Id);
            Assert.AreEqual("Form1", okResult.Content.Name);
        }

        [TestMethod]
        public async Task GetForm_ShouldReturnNotFound_WhenFormDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();

            mockRepository.Setup(repo => repo.GetFormByIdAsync(1))
                          .ReturnsAsync((GetFormResponse)null);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.GetForm(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostForm_ShouldReturnCreatedResult_WhenRequestIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var formRequest = new CreateFormRequest { Name = "Form1" };

            mockRepository.Setup(repo => repo.CreateFormAsync(formRequest))
                          .Returns(Task.CompletedTask);

            var controller = new FormsController(mockRepository.Object);

            // Setup the Url.Link to return a dummy URL
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost/api/form/form1");
            controller.Url = urlHelper.Object;

            // Act
            var result = await controller.PostForm(formRequest);

            // Assert
            var createdResult = result as CreatedNegotiatedContentResult<CreateFormRequest>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("http://localhost/api/form/form1", createdResult.Location.ToString());
            Assert.AreEqual("Form1", createdResult.Content.Name);
        }

        [TestMethod]
        public async Task PostForm_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.PostForm(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Request cannot be null", badRequestResult.Message);
        }

        [TestMethod]
        public async Task PutForm_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var formRequest = new UpdateFormRequest { Id = 1, Name = "Updated_Form1" };
            var existingForm = new GetFormResponse { Id = 1, Name = "Form1" };

            mockRepository.Setup(repo => repo.GetFormByIdAsync(1))
                          .ReturnsAsync(existingForm);
            mockRepository.Setup(repo => repo.UpdateFormAsync(formRequest))
                          .Returns(Task.CompletedTask);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.PutForm(formRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PutForm_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.PutForm(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Request cannot be null", badRequestResult.Message);
        }

        [TestMethod]
        public async Task PutForm_ShouldReturnNotFound_WhenFormDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var formRequest = new UpdateFormRequest { Id = 1, Name = "Updated_Form1" };

            mockRepository.Setup(repo => repo.GetFormByIdAsync(1))
                          .ReturnsAsync((GetFormResponse)null);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.PutForm(formRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteForm_ShouldReturnNoContent_WhenFormExists()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var existingForm = new GetFormResponse { Id = 1, Name = "Form1" };

            mockRepository.Setup(repo => repo.GetFormByIdAsync(1))
                          .ReturnsAsync(existingForm);
            mockRepository.Setup(repo => repo.DeleteFormAsync(1))
                          .Returns(Task.CompletedTask);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.DeleteForm(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteForm_ShouldReturnNotFound_WhenFormDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();

            mockRepository.Setup(repo => repo.GetFormByIdAsync(1))
                          .ReturnsAsync((GetFormResponse)null);

            var controller = new FormsController(mockRepository.Object);

            // Act
            var result = await controller.DeleteForm(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
