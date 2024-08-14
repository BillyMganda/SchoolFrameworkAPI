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
    }
}
