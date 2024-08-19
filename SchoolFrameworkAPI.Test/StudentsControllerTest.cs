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
    public class StudentsControllerTest
    {
        private Mock<IStudentRepository> _mockRepository;
        private StudentsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IStudentRepository>();
            _controller = new StudentsController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetStudents_ShouldReturnAllStudents()
        {
            // Arrange
            var students = new List<GetStudentResponse>
            {
                new GetStudentResponse { Id = 1, FirstName = "John", LastName = "Doe", FormName = "Form1" },
                new GetStudentResponse { Id = 2, FirstName = "Jane", LastName = "Smith", FormName = "Form1" }
            };

            _mockRepository.Setup(repo => repo.GetStudentsAsync())
                           .ReturnsAsync(students);

            // Act
            var result = await _controller.GetStudents();

            // Assert
            var okResult = result as OkNegotiatedContentResult<IEnumerable<GetStudentResponse>>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(2, okResult.Content.Count());
        }

        [TestMethod]
        public async Task GetStudent_ShouldReturnStudent_WhenStudentExists()
        {
            // Arrange
            var student = new GetStudentResponse { Id = 1, FirstName = "John", LastName = "Doe", FormName = "Form1" };

            _mockRepository.Setup(repo => repo.GetStudentByIdAsync(1))
                           .ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudent(1);

            // Assert
            var okResult = result as OkNegotiatedContentResult<GetStudentResponse>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual(1, okResult.Content.Id);
            Assert.AreEqual("John", okResult.Content.FirstName);
        }

        [TestMethod]
        public async Task GetStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetStudentByIdAsync(1))
                           .ReturnsAsync((GetStudentResponse)null);

            // Act
            var result = await _controller.GetStudent(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostStudent_ShouldReturnCreated_WhenStudentIsPosted()
        {
            // Arrange
            var request = new CreateStudentRequest
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2000, 1, 1),
                ParentOrGuardianFirstName = "Jane",
                ParentOrGuardianLastName = "Doe",
                ParentOrGuardianPhoneNumber = "123456789",
                ParentOrGuardianEmailAddress = "jane.doe@example.com",
                FormId = 1
            };

            _mockRepository.Setup(r => r.CreateStudentAsync(It.IsAny<CreateStudentRequest>()))
                .Returns(Task.CompletedTask);

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://testurl.com/api/students/John");
            _controller.Url = mockUrlHelper.Object;

            // Act
            var result = await _controller.PostStudent(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<CreateStudentRequest>));
            var createdResult = (CreatedNegotiatedContentResult<CreateStudentRequest>)result;
            Assert.AreEqual(request, createdResult.Content);
            Assert.IsNotNull(createdResult.Location);
            Assert.IsTrue(createdResult.Location.ToString().Contains(request.FirstName));

            _mockRepository.Verify(r => r.CreateStudentAsync(It.IsAny<CreateStudentRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task PutStudent_ShouldReturnOk_WhenStudentIsUpdated()
        {
            // Arrange
            var request = new UpdateStudentRequest
            {
                Id = 1,
                ParentOrGuardianFirstName = "John",
                ParentOrGuardianLastName = "Doe",
                ParentOrGuardianPhoneNumber = "987654321",
                ParentOrGuardianEmailAddress = "john.doe@example.com",
                FormId = 2
            };

            var existingStudent = new GetStudentResponse { Id = 1, FirstName = "John", LastName = "Doe", FormName = "Form1" };

            _mockRepository.Setup(repo => repo.GetStudentByIdAsync(1))
                           .ReturnsAsync(existingStudent);

            // Act
            var result = await _controller.PutStudent(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PutStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var request = new UpdateStudentRequest
            {
                Id = 1,
                ParentOrGuardianFirstName = "John",
                ParentOrGuardianLastName = "Doe",
                ParentOrGuardianPhoneNumber = "987654321",
                ParentOrGuardianEmailAddress = "john.doe@example.com",
                FormId = 2
            };

            _mockRepository.Setup(repo => repo.GetStudentByIdAsync(1))
                           .ReturnsAsync((GetStudentResponse)null);

            // Act
            var result = await _controller.PutStudent(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteStudent_ShouldReturnNoContent_WhenStudentIsDeleted()
        {
            // Arrange
            var student = new GetStudentResponse { Id = 1, FirstName = "John", LastName = "Doe", FormName = "Form1" };

            _mockRepository.Setup(repo => repo.GetStudentByIdAsync(1))
                           .ReturnsAsync(student);

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetStudentByIdAsync(1))
                           .ReturnsAsync((GetStudentResponse)null);

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
