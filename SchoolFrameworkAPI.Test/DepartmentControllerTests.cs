using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolFrameworkAPI.Controllers;
using SchoolFrameworkAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace SchoolFrameworkAPI.Test
{
    [TestClass]
    public class DepartmentControllerTests
    {
        private Mock<ScoolFrameworkEntities> _mockContext;
        private Mock<DbSet<Department>> _mockSet;
        private DepartmentsController _controller;
        
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
    }
}
