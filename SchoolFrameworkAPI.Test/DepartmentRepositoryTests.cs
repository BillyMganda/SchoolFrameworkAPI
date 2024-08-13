using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Test
{
    [TestClass]
    public class DepartmentRepositoryTests
    {
        [TestMethod]
        public async Task GetDepartmentsAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var mockData = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "Finance" }
            };

            var mockRepository = new Mock<IDepartmentRepository>();
            mockRepository.Setup(repo => repo.GetDepartmentsAsync())
                          .ReturnsAsync(mockData);

            // Act
            var result = await mockRepository.Object.GetDepartmentsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("HR", result.First().Name);
        }

        [TestMethod]
        public async Task GetDepartmentByIdAsync_ShouldReturnDepartment_WhenIdIsValid()
        {
            // Arrange
            var mockDepartment = new Department { Id = 1, Name = "HR" };
            var mockRepository = new Mock<IDepartmentRepository>();
            mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1))
                          .ReturnsAsync(mockDepartment);

            // Act
            var result = await mockRepository.Object.GetDepartmentByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("HR", result.Name);
        }

        [TestMethod]
        public async Task CreateDepartmentAsync_ShouldInvokeRepositoryMethod()
        {
            // Arrange
            var newDepartment = new CreateDepartmentRequest { Name = "IT" };
            var mockRepository = new Mock<IDepartmentRepository>();

            mockRepository.Setup(repo => repo.CreateDepartmentAsync(newDepartment))
                          .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.CreateDepartmentAsync(newDepartment);

            // Assert
            mockRepository.Verify(repo => repo.CreateDepartmentAsync(newDepartment), Times.Once);
        }

        [TestMethod]
        public async Task UpdateDepartmentAsync_ShouldInvokeRepositoryMethod()
        {
            // Arrange
            var updateDepartment = new UpdateDepartmentRequest { Id = 1, Name = "Updated Department" };
            var mockRepository = new Mock<IDepartmentRepository>();

            mockRepository.Setup(repo => repo.UpdateDepartmentAsync(updateDepartment))
                          .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.UpdateDepartmentAsync(updateDepartment);

            // Assert
            mockRepository.Verify(repo => repo.UpdateDepartmentAsync(updateDepartment), Times.Once);
        }

        [TestMethod]
        public async Task DeleteDepartmentAsync_ShouldInvokeRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IDepartmentRepository>();

            mockRepository.Setup(repo => repo.DeleteDepartmentAsync(1))
                          .Returns(Task.CompletedTask);

            // Act
            await mockRepository.Object.DeleteDepartmentAsync(1);

            // Assert
            mockRepository.Verify(repo => repo.DeleteDepartmentAsync(1), Times.Once);
        }
    }
}
